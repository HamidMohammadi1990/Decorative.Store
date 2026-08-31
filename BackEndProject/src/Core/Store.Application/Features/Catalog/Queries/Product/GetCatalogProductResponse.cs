using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogProductResponse
{
    public bool NotFound { get; init; }

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
    public string Description { get; init; } = string.Empty;
    public List<string> LongDescriptions { get; init; } = [];
    public List<CatalogProductImageResponse> Images { get; init; } = [];
    public List<CatalogProductFeatureResponse> Features { get; init; } = [];
    public int ReviewCount { get; init; }
    public double? AverageRating { get; init; }
    public int? SatisfactionPercent { get; init; }
    public int PurchaseCount { get; init; }
}

public record CatalogProductImageResponse
{
    public string Url { get; init; } = string.Empty;
    public string Alt { get; init; } = string.Empty;
}

public record CatalogProductFeatureResponse
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? GroupTitle { get; init; }
}
