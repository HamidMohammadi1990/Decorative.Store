namespace Store.Domain.Dtos.Catalog;

public record CatalogListingDto
{
    public bool PathNotFound { get; init; }
    public string Title { get; init; } = string.Empty;
    public List<CatalogBreadcrumbDto> Breadcrumbs { get; init; } = [];
    public List<CatalogListingProductDto> Products { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public List<CatalogListingFacetGroupDto> FacetGroups { get; init; } = [];
    public CatalogListingFacetLabelLookupDto FacetLabels { get; set; } = new();
}

public record CatalogBreadcrumbDto(string Label, string Href);

public record CatalogListingProductDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string ImageFileName { get; init; } = string.Empty;
    public string ImageAlt { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string CurrencyCode { get; init; } = "IRT";
    public decimal? CompareAtPrice { get; init; }
    public bool InStock { get; init; } = true;
    public bool OnSale { get; init; }
    public bool IsNew { get; init; }
    public string CategorySlug { get; init; } = string.Empty;
    public string SubCategorySlug { get; init; } = string.Empty;
    public Dictionary<string, List<string>> Facets { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public int ReviewCount { get; set; }
    public double? AverageRating { get; set; }
    public int PurchaseCount { get; set; }
}
