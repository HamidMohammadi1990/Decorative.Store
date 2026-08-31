namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogSearchResponse
{
    public List<CatalogSearchCategoryResponse> Categories { get; init; } = [];
    public List<CatalogSearchSubCategoryResponse> SubCategories { get; init; } = [];
    public List<CatalogListingProductResponse> Products { get; init; } = [];
}

public record CatalogSearchCategoryResponse
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public record CatalogSearchSubCategoryResponse
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string CategoryTitle { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty;
}
