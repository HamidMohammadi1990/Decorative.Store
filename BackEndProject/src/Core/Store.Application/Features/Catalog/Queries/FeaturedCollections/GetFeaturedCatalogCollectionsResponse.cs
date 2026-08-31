namespace Edition.Application.Features.Catalog.Queries;

public record GetFeaturedCatalogCollectionsResponse
{
    public List<FeaturedCatalogCollectionResponse> Collections { get; init; } = [];
}

public record FeaturedCatalogCollectionResponse
{
    public string Id { get; init; } = string.Empty;
    public string Href { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string ImageAlt { get; init; } = string.Empty;
}
