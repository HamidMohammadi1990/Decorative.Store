namespace Store.Domain.Dtos.Catalog;

public record CatalogProductDto
{
    public bool NotFound { get; init; }
    public CatalogListingProductDto? Product { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> LongDescriptions { get; init; } = [];
    public List<CatalogProductImageDto> Images { get; init; } = [];
    public List<CatalogProductFeatureDto> Features { get; init; } = [];
}

public record CatalogProductImageDto
{
    public string Url { get; init; } = string.Empty;
    public string Alt { get; init; } = string.Empty;
}

public record CatalogProductFeatureDto
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? GroupTitle { get; init; }
}
