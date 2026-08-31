using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogListingResponse
{
    public string Title { get; init; } = string.Empty;
    public bool PathNotFound { get; init; }
    public List<CatalogListingBreadcrumbResponse> Breadcrumbs { get; init; } = [];
    public List<CatalogListingProductResponse> Products { get; init; } = [];
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
}
