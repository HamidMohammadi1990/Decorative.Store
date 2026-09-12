using Edition.Application.Common.Directories;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence;
using Store.ProductImport.Models;
using Store.ProductImport.Parsing;

namespace Store.ProductImport.Import;

public sealed class ProductImportService(EditionDbContext context, string uploadsRoot)
{
    private int _faLanguageId;
    private int _enLanguageId;
    private readonly Dictionary<string, int> _subCategorySlugMap = new(StringComparer.OrdinalIgnoreCase);
    private int? _packPropertyId;
    private int? _cartonPropertyId;
    private int? _measurePropertyId;
    private int? _materialPropertyId;
    private int? _materialGlassItemId;
    private readonly Dictionary<string, int> _propertyItemCache = new(StringComparer.OrdinalIgnoreCase);
    private int? _packFeatureTypeId;
    private int? _cartonFeatureTypeId;
    private int? _purchasePriceFeatureTypeId;
    private int? _measureFeatureTypeId;

    public async Task<ImportSummary> ImportAsync(
        IReadOnlyList<ParsedInventoryProduct> products,
        bool dryRun,
        CancellationToken cancellationToken = default)
    {
        await LoadMetadataAsync(cancellationToken);
        await EnsureImportPropertiesAsync(cancellationToken);
        await EnsureFeatureTypesAsync(cancellationToken);

        var summary = new ImportSummary();
        var existingCodes = await context.Product
            .AsNoTracking()
            .Select(p => p.ProductCode)
            .ToListAsync(cancellationToken);
        var existingCodeSet = new HashSet<string>(existingCodes, StringComparer.OrdinalIgnoreCase);

        foreach (var item in products)
        {
            if (existingCodeSet.Contains(item.ProductCode))
            {
                summary.SkippedDuplicate++;
                continue;
            }

            if (!_subCategorySlugMap.TryGetValue(item.SubCategorySlug, out var subCategoryId))
            {
                summary.Failed++;
                summary.Errors.Add($"SubCategory not found: {item.SubCategorySlug} ({item.ProductCode})");
                continue;
            }

            if (dryRun)
            {
                summary.Imported++;
                continue;
            }

            try
            {
                await ImportSingleAsync(item, subCategoryId, cancellationToken);
                existingCodeSet.Add(item.ProductCode);
                summary.Imported++;
            }
            catch (Exception ex)
            {
                summary.Failed++;
                summary.Errors.Add($"{item.ProductCode}: {ex.Message}");
            }
        }

        if (!dryRun)
            await context.SaveChangesAsync(cancellationToken);

        return summary;
    }

    private async Task ImportSingleAsync(
        ParsedInventoryProduct item,
        int subCategoryId,
        CancellationToken cancellationToken)
    {
        var shortDescriptionFa = TrimToLength(item.DescriptionFa, 400);
        var shortDescriptionEn = TrimToLength(item.DescriptionEn, 400);

        var slugFa = await EnsureUniqueSlugAsync(item.SlugFa, _faLanguageId, cancellationToken);
        var slugEn = await EnsureUniqueSlugAsync(item.SlugEn, _enLanguageId, cancellationToken);

        var product = Product.Create(item.ProductCode, subCategoryId, item.PriceToman);
        product.SetInStock(!item.IsDiscontinued && item.PriceToman > 0);
        product.UpsertTranslation(_faLanguageId, TrimToLength(item.TitleFa, 150), slugFa, shortDescriptionFa);
        product.UpsertTranslation(_enLanguageId, TrimToLength(item.TitleEn, 150), slugEn, shortDescriptionEn);
        context.Product.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        var longFa = TrimToLength(item.DescriptionFa, 2500);
        var longEn = TrimToLength(item.DescriptionEn, 2500);
        context.ProductDescription.Add(ProductDescription.Create(longFa, product.Id, _faLanguageId));
        context.ProductDescription.Add(ProductDescription.Create(longEn, product.Id, _enLanguageId));

        await AssignPropertiesAsync(product, item, cancellationToken);
        AssignFeatures(product, item);

        if (item.ImageBytes is { Length: > 0 })
            await SaveProductImageAsync(product, item, cancellationToken);
    }

    private async Task AssignPropertiesAsync(
        Product product,
        ParsedInventoryProduct item,
        CancellationToken cancellationToken)
    {
        if (item.CatalogKind == ImportCatalogKind.Belza)
        {
            await AssignBelzaPropertiesAsync(product, item, cancellationToken);
            return;
        }

        if (_materialPropertyId is int materialPropertyId && _materialGlassItemId is int glassItemId)
        {
            context.ProductProperty.Add(ProductProperty.Create(
                product.Id,
                materialPropertyId,
                isActive: true,
                glassItemId));
        }

        var packValueFa = $"{item.PackQuantity} {item.PackUnitFa}".Trim();
        var packValueEn = $"{item.PackQuantity} {ProductTextHelper.TranslatePackValue(item.PackUnitFa)}".Trim();
        var packItemId = await GetOrCreatePropertyItemAsync(
            _packPropertyId!.Value,
            packValueFa,
            packValueFa,
            packValueEn,
            cancellationToken);
        context.ProductProperty.Add(ProductProperty.Create(product.Id, _packPropertyId.Value, true, packItemId));

        if (!string.IsNullOrWhiteSpace(item.MeasurementFa))
        {
            var measureEn = ProductTextHelper.TranslateTitle(item.MeasurementFa);
            var measureItemId = await GetOrCreatePropertyItemAsync(
                _measurePropertyId!.Value,
                item.MeasurementFa,
                item.MeasurementFa,
                measureEn,
                cancellationToken);
            context.ProductProperty.Add(ProductProperty.Create(product.Id, _measurePropertyId.Value, true, measureItemId));
        }
    }

    private async Task AssignBelzaPropertiesAsync(
        Product product,
        ParsedInventoryProduct item,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(item.PackQuantity))
            return;

        var cartonValueFa = $"{item.PackQuantity} {item.PackUnitFa}".Trim();
        var cartonValueEn = $"{item.PackQuantity} {ProductTextHelper.TranslatePackValue(item.PackUnitFa)}".Trim();
        var cartonItemId = await GetOrCreatePropertyItemAsync(
            _cartonPropertyId!.Value,
            cartonValueFa,
            cartonValueFa,
            cartonValueEn,
            cancellationToken);
        context.ProductProperty.Add(ProductProperty.Create(product.Id, _cartonPropertyId.Value, true, cartonItemId));
    }

    private void AssignFeatures(Product product, ParsedInventoryProduct item)
    {
        if (item.CatalogKind == ImportCatalogKind.Belza)
        {
            AssignBelzaFeatures(product, item);
            return;
        }

        var packValueFa = $"{item.PackQuantity} {item.PackUnitFa}".Trim();
        product.AddFeature(ProductFeature.Create(product.Id, _packFeatureTypeId!.Value, packValueFa));

        if (!string.IsNullOrWhiteSpace(item.MeasurementFa))
            product.AddFeature(ProductFeature.Create(product.Id, _measureFeatureTypeId!.Value, item.MeasurementFa));
    }

    private void AssignBelzaFeatures(Product product, ParsedInventoryProduct item)
    {
        if (!string.IsNullOrWhiteSpace(item.PackQuantity))
        {
            var cartonValueFa = $"{item.PackQuantity} {item.PackUnitFa}".Trim();
            product.AddFeature(ProductFeature.Create(product.Id, _cartonFeatureTypeId!.Value, cartonValueFa));
        }

        if (item.PurchasePriceToman > 0)
        {
            var purchaseValue = item.PurchasePriceToman.ToString("0", System.Globalization.CultureInfo.InvariantCulture);
            product.AddFeature(ProductFeature.Create(product.Id, _purchasePriceFeatureTypeId!.Value, purchaseValue));
        }
    }

    private async Task SaveProductImageAsync(
        Product product,
        ParsedInventoryProduct item,
        CancellationToken cancellationToken)
    {
        var extension = string.IsNullOrWhiteSpace(item.ImageExtension) ? ".jpg" : item.ImageExtension;
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var relativeDirectory = ProductDirectory.ProductImage.Replace("wwwroot", string.Empty, StringComparison.Ordinal)
            .TrimStart('/', '\\');
        var absoluteDirectory = Path.Combine(uploadsRoot, relativeDirectory.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(absoluteDirectory);

        var absolutePath = Path.Combine(absoluteDirectory, fileName);
        await File.WriteAllBytesAsync(absolutePath, item.ImageBytes!, cancellationToken);

        var imageTitleFa = TrimToLength(item.TitleFa, 30);
        var imageTitleEn = TrimToLength(item.TitleEn, 30);
        var productFile = ProductFile.Create(product.Id, fileName, isMain: true);
        productFile.UpsertTranslation(_faLanguageId, imageTitleFa);
        productFile.UpsertTranslation(_enLanguageId, imageTitleEn);
        context.ProductFile.Add(productFile);
    }

    private async Task<int> GetOrCreatePropertyItemAsync(
        int propertyId,
        string codeSeed,
        string faTitle,
        string enTitle,
        CancellationToken cancellationToken)
    {
        var code = SlugifyPropertyCode(codeSeed);
        var cacheKey = $"{propertyId}:{code}";
        if (_propertyItemCache.TryGetValue(cacheKey, out var cachedId))
            return cachedId;

        var existing = await context.PropertyItem
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.Code == code, cancellationToken);

        if (existing is not null)
        {
            _propertyItemCache[cacheKey] = existing.Id;
            return existing.Id;
        }

        var item = PropertyItem.Create(code, propertyId, priority: 0);
        item.UpsertTranslation(_faLanguageId, TrimToLength(faTitle, 30));
        item.UpsertTranslation(_enLanguageId, TrimToLength(enTitle, 30));
        context.PropertyItem.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        _propertyItemCache[cacheKey] = item.Id;
        return item.Id;
    }

    private async Task LoadMetadataAsync(CancellationToken cancellationToken)
    {
        _faLanguageId = await context.Language
            .Where(x => x.Code == "fa-IR")
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        _enLanguageId = await context.Language
            .Where(x => x.Code == "en-US")
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var subCategories = await context.SubCategoryTranslation
            .AsNoTracking()
            .Where(x => x.LanguageId == _faLanguageId)
            .Select(x => new { x.Slug, x.SubCategoryId })
            .ToListAsync(cancellationToken);

        foreach (var entry in subCategories)
            _subCategorySlugMap[entry.Slug] = entry.SubCategoryId;
    }

    private async Task EnsureImportPropertiesAsync(CancellationToken cancellationToken)
    {
        _packPropertyId = await EnsurePropertyAsync(
            "pack-quantity",
            "تعداد در بسته",
            "Pack quantity",
            cancellationToken);

        _cartonPropertyId = await EnsurePropertyAsync(
            "carton-quantity",
            "تعداد در کارتن",
            "Carton quantity",
            cancellationToken);

        _measurePropertyId = await EnsurePropertyAsync(
            "measurement-unit",
            "واحد اندازه‌گیری",
            "Measurement unit",
            cancellationToken);

        _materialPropertyId = await context.Property
            .AsNoTracking()
            .Where(x => x.Code == "material")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (_materialPropertyId is int materialPropertyId)
        {
            _materialGlassItemId = await context.PropertyItem
                .AsNoTracking()
                .Where(x => x.PropertyId == materialPropertyId && x.Code == "glass")
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    private async Task<int> EnsurePropertyAsync(
        string code,
        string faTitle,
        string enTitle,
        CancellationToken cancellationToken)
    {
        var existing = await context.Property.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
        if (existing is not null)
            return existing.Id;

        var categoryId = await context.PropertyCategory
            .AsNoTracking()
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var property = Property.Create(PropertyType.Select, parentId: null, code, categoryId, priority: 50);
        property.UpsertTranslation(_faLanguageId, faTitle, description: null);
        property.UpsertTranslation(_enLanguageId, enTitle, description: null);
        context.Property.Add(property);
        await context.SaveChangesAsync(cancellationToken);
        return property.Id;
    }

    private async Task EnsureFeatureTypesAsync(CancellationToken cancellationToken)
    {
        _packFeatureTypeId = await EnsureFeatureTypeAsync("تعداد در بسته", cancellationToken);
        _cartonFeatureTypeId = await EnsureFeatureTypeAsync("تعداد در کارتن", cancellationToken);
        _purchasePriceFeatureTypeId = await EnsureFeatureTypeAsync("قیمت خرید", cancellationToken);
        _measureFeatureTypeId = await EnsureFeatureTypeAsync("واحد اندازه‌گیری", cancellationToken);
    }

    private async Task<int> EnsureFeatureTypeAsync(string name, CancellationToken cancellationToken)
    {
        var existing = await context.ProductFeatureType
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        if (existing is not null)
            return existing.Id;

        var featureType = ProductFeatureType.Create(
            name,
            ProductFeatureTypeCode.DisplayOnMenu,
            ProductFeatureDataType.String,
            description: null,
            isActive: true);
        context.ProductFeatureType.Add(featureType);
        await context.SaveChangesAsync(cancellationToken);
        return featureType.Id;
    }

    private static string SlugifyPropertyCode(string input)
    {
        var code = new string(input
            .Trim()
            .ToLowerInvariant()
            .Select(ch => char.IsLetterOrDigit(ch) ? ch : '-')
            .ToArray());
        code = string.Join('-', code.Split('-', StringSplitOptions.RemoveEmptyEntries));
        if (code.Length > 30)
            code = code[..24].Trim('-');
        return string.IsNullOrWhiteSpace(code) ? "value" : code;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string slug,
        int languageId,
        CancellationToken cancellationToken)
    {
        var candidate = slug;
        var suffix = 1;
        while (await context.ProductTranslation.AnyAsync(
                   x => x.LanguageId == languageId && x.Slug == candidate,
                   cancellationToken))
        {
            candidate = TrimToLength($"{slug}-{suffix}", 150);
            suffix++;
        }

        return candidate;
    }

    private static string TrimToLength(string value, int max)
        => value.Length <= max ? value : value[..max];
}

public sealed class ImportSummary
{
    public int Imported { get; set; }
    public int SkippedDuplicate { get; set; }
    public int Failed { get; set; }
    public List<string> Errors { get; } = [];
}
