using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogListingResponse
{
    public string Title { get; init; } = string.Empty;
    public bool PathNotFound { get; init; }
    public List<CatalogListingBreadcrumbResponse> Breadcrumbs { get; init; } = [];
    public List<CatalogListingProductResponse> Products { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public List<CatalogListingFacetGroupResponse> FacetGroups { get; init; } = [];
}

public record CatalogListingBreadcrumbResponse
{
    public string Label { get; init; } = string.Empty;
    public string Href { get; init; } = string.Empty;
}

public record CatalogListingProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string ImageAlt { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string CurrencyCode { get; init; } = "IRT";
    public decimal? CompareAtPrice { get; init; }
    public bool InStock { get; init; }
    public bool OnSale { get; init; }
    public bool IsNew { get; init; }
    public string CategorySlug { get; init; } = string.Empty;
    public string SubCategorySlug { get; init; } = string.Empty;
    public Dictionary<string, List<string>> Facets { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public int ReviewCount { get; init; }
    public double? AverageRating { get; init; }
    public int PurchaseCount { get; init; }
}

public record CatalogListingFacetGroupResponse
{
    public string Id { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Type { get; init; } = "checkbox";
    public List<CatalogListingFacetOptionResponse> Options { get; init; } = [];
    public CatalogListingPriceRangeResponse? Range { get; init; }
}

public record CatalogListingFacetOptionResponse
{
    public string Value { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public int Count { get; init; }
    public string? Swatch { get; init; }
}

public record CatalogListingPriceRangeResponse
{
    public decimal Min { get; init; }
    public decimal Max { get; init; }
    public decimal Step { get; init; } = 1;
    public decimal? SelectedMin { get; init; }
    public decimal? SelectedMax { get; init; }
}
