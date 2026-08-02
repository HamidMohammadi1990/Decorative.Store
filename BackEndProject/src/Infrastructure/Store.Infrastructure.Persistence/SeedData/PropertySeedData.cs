using System.Text.Json;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence.SeedData.Models;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class PropertySeedData
{
    private const string FaFileName = "property-facets.fa.json";
    private const string EnFileName = "property-facets.en.json";
    private const string ProductsFaFileName = "products.fa.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static PropertyFacetCatalogSeed Catalog { get; } = BuildCatalog();
    public static IReadOnlyList<ProductFacetAssignmentSeedItem> ProductAssignments { get; } = BuildProductAssignments();

    private static PropertyFacetCatalogSeed BuildCatalog()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var faJson = File.ReadAllText(Path.Combine(seedDataPath, FaFileName));
        var enJson = File.ReadAllText(Path.Combine(seedDataPath, EnFileName));

        var faRoot = JsonSerializer.Deserialize<PropertyFacetsSeedRoot>(faJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {FaFileName}.");
        var enRoot = JsonSerializer.Deserialize<PropertyFacetsSeedRoot>(enJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {EnFileName}.");

        var usedValueCodes = CollectUsedValueCodes(seedDataPath);
        var enProperties = enRoot.Properties.ToDictionary(x => x.Code, StringComparer.OrdinalIgnoreCase);

        var properties = new List<PropertyFacetPropertySeedItem>();
        foreach (var faProperty in faRoot.Properties)
        {
            if (!enProperties.TryGetValue(faProperty.Code, out var enProperty))
                throw new InvalidOperationException($"Property '{faProperty.Code}' exists in FA seed but not in EN seed.");

            var values = new List<PropertyFacetValueSeedItem>();
            var priority = 1;

            foreach (var (valueCode, faTitle) in faProperty.Values)
            {
                if (!usedValueCodes.TryGetValue(faProperty.Code, out var usedCodes) || !usedCodes.Contains(valueCode))
                    continue;

                if (!enProperty.Values.TryGetValue(valueCode, out var enTitle))
                    throw new InvalidOperationException(
                        $"Value '{valueCode}' for property '{faProperty.Code}' exists in FA seed but not in EN seed.");

                values.Add(new PropertyFacetValueSeedItem(
                    Code: valueCode,
                    FaTitle: faTitle.Trim(),
                    EnTitle: enTitle.Trim(),
                    Priority: priority++));
            }

            properties.Add(new PropertyFacetPropertySeedItem(
                Code: faProperty.Code,
                FaTitle: faProperty.Title.Trim(),
                EnTitle: enProperty.Title.Trim(),
                Priority: properties.Count + 1,
                Values: values));
        }

        return new PropertyFacetCatalogSeed(
            CategoryCode: faRoot.Category.Code,
            FaCategoryTitle: faRoot.Category.Title.Trim(),
            EnCategoryTitle: enRoot.Category.Title.Trim(),
            Properties: properties);
    }

    private static IReadOnlyList<ProductFacetAssignmentSeedItem> BuildProductAssignments()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var productsJson = File.ReadAllText(Path.Combine(seedDataPath, ProductsFaFileName));
        var products = JsonSerializer.Deserialize<List<ProductFacetSeedJsonItem>>(productsJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {ProductsFaFileName}.");

        var assignments = new List<ProductFacetAssignmentSeedItem>();

        foreach (var product in products)
        {
            var productCode = product.Id.Trim().ToUpperInvariant();

            foreach (var (propertyCode, values) in product.Facets)
            {
                foreach (var valueCode in values)
                {
                    assignments.Add(new ProductFacetAssignmentSeedItem(
                        ProductCode: productCode,
                        PropertyCode: propertyCode,
                        ValueCode: valueCode));
                }
            }
        }

        return assignments;
    }

    private static Dictionary<string, HashSet<string>> CollectUsedValueCodes(string seedDataPath)
    {
        var productsJson = File.ReadAllText(Path.Combine(seedDataPath, ProductsFaFileName));
        var products = JsonSerializer.Deserialize<List<ProductFacetSeedJsonItem>>(productsJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {ProductsFaFileName}.");

        var used = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var product in products)
        {
            foreach (var (propertyCode, values) in product.Facets)
            {
                if (!used.TryGetValue(propertyCode, out var valueSet))
                {
                    valueSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    used[propertyCode] = valueSet;
                }

                foreach (var value in values)
                    valueSet.Add(value);
            }
        }

        return used;
    }
}

internal sealed record PropertyFacetCatalogSeed(
    string CategoryCode,
    string FaCategoryTitle,
    string EnCategoryTitle,
    IReadOnlyList<PropertyFacetPropertySeedItem> Properties);

internal sealed record PropertyFacetPropertySeedItem(
    string Code,
    string FaTitle,
    string EnTitle,
    int Priority,
    IReadOnlyList<PropertyFacetValueSeedItem> Values);

internal sealed record PropertyFacetValueSeedItem(
    string Code,
    string FaTitle,
    string EnTitle,
    int Priority);
