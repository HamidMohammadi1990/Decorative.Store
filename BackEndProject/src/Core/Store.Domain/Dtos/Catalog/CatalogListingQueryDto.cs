namespace Store.Domain.Dtos.Catalog;

public record CatalogListingQueryDto
{
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public bool? InStock { get; init; }
    public bool? OnSale { get; init; }
    public bool? IsNew { get; init; }
    public double? MinRating { get; init; }
    public string? Sort { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public List<string> PriceBuckets { get; init; } = [];
    public Dictionary<string, List<string>> AttributeFilters { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}

public record CatalogListingResult
{
    public CatalogListingDto Listing { get; init; } = new();
    public List<CatalogListingProductDto> FacetScopeProducts { get; init; } = [];
}
