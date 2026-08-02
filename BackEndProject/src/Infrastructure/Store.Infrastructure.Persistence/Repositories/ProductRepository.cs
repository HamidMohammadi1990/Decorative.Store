using System.Linq.Expressions;
using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Catalog;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Product>(context), IProductRepository
{
    public Task<Product?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Product
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Product?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Product
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsProductCodeAsync(string productCode, int? excludeProductId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = productCode.Trim();
        return Context.Product.AnyAsync(
            x => x.ProductCode == normalizedCode && (!excludeProductId.HasValue || x.Id != excludeProductId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeProductId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.ProductTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeProductId.HasValue || x.ProductId != excludeProductId.Value),
            cancellationToken);
    }

    public async Task<ProductSummaryDto?> GetProductSummaryByIdAsync(int id)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        return await Context
            .Product
            .Include(x => x.ProductFiles)
                .ThenInclude(f => f.Translations)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ProductSummaryDto
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                Slug = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Description = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? string.Empty,
                SubCategoryTitle = x.SubCategory!.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.SubCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Images = x.ProductFiles!
                    .Select(file => new CheckoutProductImageDto
                    {
                        Url = file.FileName,
                        Title = file.Translations
                                .Where(t => t.LanguageId == languageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? file.Translations
                                .Where(t => t.LanguageId == defaultLanguageId)
                                .Select(t => t.Title)
                                .FirstOrDefault()
                            ?? string.Empty
                    }).ToList()
            })
            .SingleOrDefaultAsync();
    }

    public async Task<PagedResult<GetAllProductResponseDto>> GetAllAsync(
        GetAllProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var productSource = Context.Product
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from product in productSource
            join subCategory in Context.SubCategory on product.SubCategoryId equals subCategory.Id
            join category in Context.Category on subCategory.CategoryId equals category.Id
            select new { product, subCategory, category };

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.category.Id == request.CategoryId.Value);

        if (request.SubCategoryId.HasValue)
            query = query.Where(x => x.product.SubCategoryId == request.SubCategoryId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.product.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.ProductCode))
            query = query.Where(x => x.product.ProductCode.Contains(request.ProductCode));

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.product.Translations.Any(t => t.Title.Contains(request.Title)));

        if (!string.IsNullOrWhiteSpace(request.Slug))
            query = query.Where(x => x.product.Translations.Any(t => t.Slug == request.Slug));

        if (!string.IsNullOrWhiteSpace(request.CategorySlug))
            query = query.Where(x => x.category.Translations.Any(t => t.Slug == request.CategorySlug));

        if (!string.IsNullOrWhiteSpace(request.SubCategorySlug))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Slug == request.SubCategorySlug));

        var result = await query
            .Select(x => new GetAllProductResponseDto
            {
                Id = x.product.Id,
                ProductCode = x.product.ProductCode,
                IsActive = x.product.IsActive,
                CreatedOnUtc = x.product.CreatedOnUtc,
                SubCategoryId = x.product.SubCategoryId,
                Translations = x.product.Translations
                    .Select(t => new ProductTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug,
                        Description = t.Description
                    })
                    .ToList()
            })
            .Distinct()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<CatalogListingDto> GetCatalogListingByPathAsync(
        string catalogPath,
        CancellationToken cancellationToken = default)
    {
        var normalizedPath = catalogPath.Trim().Trim('/').ToLowerInvariant();
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(normalizedPath))
        {
            return new CatalogListingDto
            {
                PathNotFound = true,
                Breadcrumbs =
                [
                    new CatalogBreadcrumbDto(
                        await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken),
                        "/")
                ]
            };
        }

        var subCategoryMatch = await (
                from translation in Context.SubCategoryTranslation.AsNoTracking()
                join subCategory in Context.SubCategory.AsNoTracking() on translation.SubCategoryId equals subCategory.Id
                join category in Context.Category.AsNoTracking() on subCategory.CategoryId equals category.Id
                where subCategory.IsActive
                      && category.IsActive
                      && translation.Slug.ToLower() == normalizedPath
                select new
                {
                    SubCategoryId = subCategory.Id,
                    CategoryId = category.Id
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategoryMatch is not null)
        {
            var categoryTranslation = await ResolveCategoryTranslationAsync(
                subCategoryMatch.CategoryId,
                languageId,
                defaultLanguageId,
                cancellationToken);

            var subCategoryTranslation = await ResolveSubCategoryTranslationAsync(
                subCategoryMatch.SubCategoryId,
                languageId,
                defaultLanguageId,
                cancellationToken);

            var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
            var breadcrumbs = new List<CatalogBreadcrumbDto>
            {
                new(homeLabel, "/"),
                new(categoryTranslation.Title, $"/{categoryTranslation.Slug}"),
                new(subCategoryTranslation.Title, $"/{subCategoryTranslation.Slug}")
            };

            var products = await LoadCatalogProductsAsync(
                [subCategoryMatch.SubCategoryId],
                languageId,
                defaultLanguageId,
                cancellationToken);

            return new CatalogListingDto
            {
                Title = subCategoryTranslation.Title,
                Breadcrumbs = breadcrumbs,
                Products = products
            };
        }

        var categoryMatch = await (
                from translation in Context.CategoryTranslation.AsNoTracking()
                join category in Context.Category.AsNoTracking() on translation.CategoryId equals category.Id
                where category.IsActive && translation.Slug.ToLower() == normalizedPath
                select category.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryMatch != default)
        {
            var categoryTranslation = await ResolveCategoryTranslationAsync(
                categoryMatch,
                languageId,
                defaultLanguageId,
                cancellationToken);

            var subCategoryIds = await Context.SubCategory
                .AsNoTracking()
                .Where(x => x.IsActive && x.CategoryId == categoryMatch)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var homeLabel = await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken);
            var breadcrumbs = new List<CatalogBreadcrumbDto>
            {
                new(homeLabel, "/"),
                new(categoryTranslation.Title, $"/{categoryTranslation.Slug}")
            };

            var products = await LoadCatalogProductsAsync(
                subCategoryIds,
                languageId,
                defaultLanguageId,
                cancellationToken);

            return new CatalogListingDto
            {
                Title = categoryTranslation.Title,
                Breadcrumbs = breadcrumbs,
                Products = products
            };
        }

        return new CatalogListingDto
        {
            PathNotFound = true,
            Title = normalizedPath,
            Breadcrumbs =
            [
                new CatalogBreadcrumbDto(
                    await ResolveHomeLabelAsync(languageId, defaultLanguageId, cancellationToken),
                    "/")
            ]
        };
    }

    public async Task<CatalogProductDto?> GetCatalogProductBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedSlug))
            return new CatalogProductDto { NotFound = true };

        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var product = await Context.Product
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Translations)
            .Include(x => x.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(x => x.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(x => x.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .Include(x => x.ProductFeatures)
            .ThenInclude(feature => feature.ProductFeatureType)
            .FirstOrDefaultAsync(
                x => x.Translations.Any(t => t.Slug.ToLower() == normalizedSlug),
                cancellationToken);

        if (product is null)
            return new CatalogProductDto { NotFound = true };

        var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
        var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
        var description = ResolveProductTranslation(
            product.Translations,
            languageId,
            defaultLanguageId,
            t => t.Description);

        var mainFile = product.ProductFiles
            .OrderByDescending(file => file.IsMain)
            .ThenBy(file => file.Id)
            .FirstOrDefault();
        var imageAlt = mainFile is null
            ? title
            : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);

        var price = product.Price;
        var compareAtPrice = product.CompareAtPrice;
        var categorySlug = ResolveCategoryTranslationValue(
            product.SubCategory.Category.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);
        var subCategorySlug = ResolveSubCategoryTranslationValue(
            product.SubCategory.Translations,
            languageId,
            defaultLanguageId,
            translation => translation.Slug);

        var summary = new CatalogListingProductDto
        {
            Id = product.Id,
            Title = title,
            Slug = productSlug,
            ImageFileName = mainFile?.FileName ?? string.Empty,
            ImageAlt = imageAlt,
            Price = price,
            CompareAtPrice = compareAtPrice,
            OnSale = compareAtPrice.HasValue && compareAtPrice > price,
            CategorySlug = categorySlug,
            SubCategorySlug = subCategorySlug
        };

        var longDescriptions = await Context.ProductDescription
            .AsNoTracking()
            .Where(x => x.ProductId == product.Id && x.LanguageId == languageId)
            .Select(x => x.Description)
            .ToListAsync(cancellationToken);

        if (longDescriptions.Count == 0)
        {
            longDescriptions = await Context.ProductDescription
                .AsNoTracking()
                .Where(x => x.ProductId == product.Id && x.LanguageId == defaultLanguageId)
                .Select(x => x.Description)
                .ToListAsync(cancellationToken);
        }

        var images = product.ProductFiles
            .OrderByDescending(file => file.IsMain)
            .ThenBy(file => file.Id)
            .Select(file => new CatalogProductImageDto
            {
                Url = file.FileName,
                Alt = ResolveProductFileTranslation(file.Translations, languageId, defaultLanguageId)
            })
            .ToList();

        var features = product.ProductFeatures
            .Where(feature => feature.ProductFeatureType.IsActive)
            .OrderBy(feature => feature.ProductFeatureType.Name)
            .Select(feature => new CatalogProductFeatureDto
            {
                Label = feature.ProductFeatureType.Name,
                Value = feature.Value,
                GroupTitle = feature.ProductFeatureType.Description
            })
            .ToList();

        return new CatalogProductDto
        {
            NotFound = false,
            Product = summary,
            Description = description,
            LongDescriptions = longDescriptions,
            Images = images,
            Features = features
        };
    }

    public async Task<List<CatalogListingProductDto>> GetRelatedCatalogProductsBySlugAsync(
        string slug,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalizedSlug) || limit <= 0)
            return [];

        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var currentProduct = await Context.Product
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(
                x => x.Translations.Any(t => t.Slug.ToLower() == normalizedSlug),
                cancellationToken);

        if (currentProduct is null)
            return [];

        var rows = await Context.Product
            .AsNoTracking()
            .Where(product =>
                product.IsActive &&
                product.Id != currentProduct.Id &&
                product.SubCategoryId == currentProduct.SubCategoryId)
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .OrderBy(product => product.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return rows.Select(product =>
        {
            var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
            var productSlug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
            var mainFile = product.ProductFiles
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .FirstOrDefault();
            var imageAlt = mainFile is null
                ? title
                : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);
            var price = product.Price;
            var compareAtPrice = product.CompareAtPrice;
            var categorySlug = ResolveCategoryTranslationValue(
                product.SubCategory.Category.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);
            var subCategorySlug = ResolveSubCategoryTranslationValue(
                product.SubCategory.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);

            return new CatalogListingProductDto
            {
                Id = product.Id,
                Title = title,
                Slug = productSlug,
                ImageFileName = mainFile?.FileName ?? string.Empty,
                ImageAlt = imageAlt,
                Price = price,
                CompareAtPrice = compareAtPrice,
                OnSale = compareAtPrice.HasValue && compareAtPrice > price,
                CategorySlug = categorySlug,
                SubCategorySlug = subCategorySlug
            };
        }).ToList();
    }

    private async Task<List<CatalogListingProductDto>> LoadCatalogProductsAsync(
        IReadOnlyCollection<int> subCategoryIds,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        if (subCategoryIds.Count == 0)
            return [];

        var rows = await Context.Product
            .AsNoTracking()
            .Where(product => product.IsActive && subCategoryIds.Contains(product.SubCategoryId))
            .Include(product => product.Translations)
            .Include(product => product.ProductFiles.Where(file => file.IsActive))
            .ThenInclude(file => file.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Translations)
            .Include(product => product.SubCategory)
            .ThenInclude(subCategory => subCategory.Category)
            .ThenInclude(category => category.Translations)
            .OrderBy(product => product.Id)
            .ToListAsync(cancellationToken);

        return rows.Select(product =>
        {
            var title = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Title);
            var slug = ResolveProductTranslation(product.Translations, languageId, defaultLanguageId, t => t.Slug);
            var mainFile = product.ProductFiles
                .OrderByDescending(file => file.IsMain)
                .ThenBy(file => file.Id)
                .FirstOrDefault();
            var imageAlt = mainFile is null
                ? title
                : ResolveProductFileTranslation(mainFile.Translations, languageId, defaultLanguageId);
            var price = product.Price;
            var compareAtPrice = product.CompareAtPrice;
            var categorySlug = ResolveCategoryTranslationValue(
                product.SubCategory.Category.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);
            var subCategorySlug = ResolveSubCategoryTranslationValue(
                product.SubCategory.Translations,
                languageId,
                defaultLanguageId,
                translation => translation.Slug);

            return new CatalogListingProductDto
            {
                Id = product.Id,
                Title = title,
                Slug = slug,
                ImageFileName = mainFile?.FileName ?? string.Empty,
                ImageAlt = imageAlt,
                Price = price,
                CompareAtPrice = compareAtPrice,
                OnSale = compareAtPrice.HasValue && compareAtPrice > price,
                CategorySlug = categorySlug,
                SubCategorySlug = subCategorySlug
            };
        }).ToList();
    }

    private static string ResolveProductTranslation(
        IEnumerable<ProductTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<ProductTranslation, string> selector)
    {
        var list = translations as IList<ProductTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private static string ResolveProductFileTranslation(
        IEnumerable<ProductFileTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<ProductFileTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Title
               ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Title
               ?? list.FirstOrDefault()?.Title
               ?? string.Empty;
    }

    private static string ResolveCategoryTranslationValue(
        IEnumerable<CategoryTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<CategoryTranslation, string> selector)
    {
        var list = translations as IList<CategoryTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private static string ResolveSubCategoryTranslationValue(
        IEnumerable<SubCategoryTranslation> translations,
        int languageId,
        int defaultLanguageId,
        Func<SubCategoryTranslation, string> selector)
    {
        var list = translations as IList<SubCategoryTranslation> ?? translations.ToList();
        var translation = list.FirstOrDefault(t => t.LanguageId == languageId)
                          ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)
                          ?? list.FirstOrDefault();
        return translation is null ? string.Empty : selector(translation);
    }

    private async Task<(string Title, string Slug)> ResolveCategoryTranslationAsync(
        int categoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var translations = await Context.CategoryTranslation
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync(cancellationToken);

        var current = translations.FirstOrDefault(x => x.LanguageId == languageId)
                      ?? translations.FirstOrDefault(x => x.LanguageId == defaultLanguageId)
                      ?? translations.FirstOrDefault();

        return current is null
            ? (string.Empty, string.Empty)
            : (current.Title, current.Slug);
    }

    private async Task<(string Title, string Slug)> ResolveSubCategoryTranslationAsync(
        int subCategoryId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var translations = await Context.SubCategoryTranslation
            .AsNoTracking()
            .Where(x => x.SubCategoryId == subCategoryId)
            .ToListAsync(cancellationToken);

        var current = translations.FirstOrDefault(x => x.LanguageId == languageId)
                      ?? translations.FirstOrDefault(x => x.LanguageId == defaultLanguageId)
                      ?? translations.FirstOrDefault();

        return current is null
            ? (string.Empty, string.Empty)
            : (current.Title, current.Slug);
    }

    private async Task<string> ResolveHomeLabelAsync(
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken)
    {
        var language = await languageRegistry.GetByIdAsync(languageId, cancellationToken)
                       ?? await languageRegistry.GetByIdAsync(defaultLanguageId, cancellationToken);
        var code = language?.Code ?? "en-US";
        return code.StartsWith("fa", StringComparison.OrdinalIgnoreCase) ? "خانه" : "Home";
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken = default)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
