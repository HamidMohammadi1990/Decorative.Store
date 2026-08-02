using System.Text.Json.Serialization;

namespace Store.Infrastructure.Persistence.SeedData.Models;

internal sealed class PropertyFacetsSeedRoot
{
    public PropertyFacetCategorySeedItem Category { get; init; } = default!;
    public List<PropertyFacetPropertySeedItem> Properties { get; init; } = [];
}

internal sealed class PropertyFacetCategorySeedItem
{
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
}

internal sealed class PropertyFacetPropertySeedItem
{
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public Dictionary<string, string> Values { get; init; } = [];
}

internal sealed class ProductFacetSeedJsonItem
{
    public string Id { get; init; } = default!;

    [JsonPropertyName("facets")]
    public Dictionary<string, List<string>> Facets { get; init; } = [];
}

internal sealed record ProductFacetAssignmentSeedItem(
    string ProductCode,
    string PropertyCode,
    string ValueCode);
