using System.Text.Json;
using Store.Infrastructure.Persistence.SeedData.Models;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class ProductSeedData
{
    private const string FaFileName = "products.fa.json";
    private const string EnFileName = "products.en.json";
    private const int MaxDescriptionLength = 400;
    private const int MaxImageTitleLength = 30;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Dictionary<string, string> SubCategorySlugOverrides =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["sectionals"] = "sofas/chaise-corner"
        };

    public static IReadOnlyList<ProductSeedItem> Items { get; } = Build();

    private static IReadOnlyList<ProductSeedItem> Build()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var faJson = File.ReadAllText(Path.Combine(seedDataPath, FaFileName));
        var enJson = File.ReadAllText(Path.Combine(seedDataPath, EnFileName));

        var faProducts = JsonSerializer.Deserialize<List<ProductSeedJsonItem>>(faJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {FaFileName}.");
        var enProducts = JsonSerializer.Deserialize<List<ProductSeedJsonItem>>(enJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {EnFileName}.");

        var enById = enProducts.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        var items = new List<ProductSeedItem>();

        foreach (var faProduct in faProducts)
        {
            if (!enById.TryGetValue(faProduct.Id, out var enProduct))
                throw new InvalidOperationException($"Product '{faProduct.Id}' exists in FA seed but not in EN seed.");

            if (string.IsNullOrWhiteSpace(faProduct.SubCategorySlug))
                throw new InvalidOperationException($"Product '{faProduct.Id}' is missing subcategorySlug.");

            var subCategorySlug = ResolveSubCategorySlug(faProduct.SubCategorySlug, faProduct.CategorySlugs);

            var imageBaseName = ResolveImageBaseName(faProduct.Image.Src);

            items.Add(new ProductSeedItem(
                ProductCode: faProduct.Id.Trim().ToUpperInvariant(),
                FaTitle: faProduct.Title.Trim(),
                EnTitle: enProduct.Title.Trim(),
                FaSlug: faProduct.Slug.Trim(),
                EnSlug: enProduct.Slug.Trim(),
                FaDescription: TruncateDescription(faProduct.Image.Alt),
                EnDescription: TruncateDescription(enProduct.Image.Alt),
                SubCategorySlug: subCategorySlug,
                Price: faProduct.Price?.Amount ?? 0m,
                CompareAtPrice: faProduct.CompareAtPrice?.Amount,
                ImageBaseName: imageBaseName,
                FaImageTitle: TruncateImageTitle(faProduct.Image.Alt),
                EnImageTitle: TruncateImageTitle(enProduct.Image.Alt),
                InStock: faProduct.InStock));
        }

        return items;
    }

    private static string ResolveSubCategorySlug(string subcategorySlug, IReadOnlyList<string> categorySlugs)
    {
        if (SubCategorySlugOverrides.TryGetValue(subcategorySlug, out var overrideSlug))
            return overrideSlug;

        var allSubCategories = CategorySeedData.Items
            .SelectMany(category => category.SubCategories.Select(sub => new { Category = category, Sub = sub }))
            .ToList();

        var candidates = allSubCategories
            .Where(x => string.Equals(x.Sub.FaSlug, subcategorySlug, StringComparison.OrdinalIgnoreCase)
                        || x.Sub.FaSlug.EndsWith('/' + subcategorySlug, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (candidates.Count == 0)
            throw new InvalidOperationException($"No subcategory match found for slug '{subcategorySlug}'.");

        if (candidates.Count == 1)
            return candidates[0].Sub.FaSlug;

        foreach (var categorySlug in categorySlugs)
        {
            var match = candidates.FirstOrDefault(x =>
                x.Sub.FaSlug.StartsWith(categorySlug + "/", StringComparison.OrdinalIgnoreCase));

            if (match is not null)
                return match.Sub.FaSlug;
        }

        foreach (var categorySlug in categorySlugs)
        {
            var category = CategorySeedData.Items.FirstOrDefault(x =>
                string.Equals(x.Code, categorySlug, StringComparison.OrdinalIgnoreCase)
                || string.Equals(x.FaSlug, categorySlug, StringComparison.OrdinalIgnoreCase)
                || string.Equals(x.EnSlug, categorySlug, StringComparison.OrdinalIgnoreCase));

            if (category is null)
                continue;

            var match = candidates.FirstOrDefault(x =>
                category.SubCategories.Any(sub => sub.Code == x.Sub.Code));

            if (match is not null)
                return match.Sub.FaSlug;
        }

        return candidates[0].Sub.FaSlug;
    }

    private static string ResolveImageBaseName(string imageSrc)
    {
        if (string.IsNullOrWhiteSpace(imageSrc))
            throw new InvalidOperationException("Product image src is required.");

        var fileName = Path.GetFileName(imageSrc.Trim().TrimStart('/'));
        var baseName = Path.GetFileNameWithoutExtension(fileName);

        if (string.IsNullOrWhiteSpace(baseName))
            throw new InvalidOperationException($"Invalid product image src '{imageSrc}'.");

        return baseName;
    }

    private static string TruncateDescription(string value)
    {
        var description = value.Trim();
        return description.Length <= MaxDescriptionLength
            ? description
            : description[..MaxDescriptionLength];
    }

    private static string TruncateImageTitle(string value)
    {
        var title = value.Trim();
        return title.Length <= MaxImageTitleLength
            ? title
            : title[..MaxImageTitleLength];
    }
}
