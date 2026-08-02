namespace Edition.Application.Features.Catalog.Queries;

public record GetRelatedCatalogProductsResponse
{
    public List<CatalogListingProductResponse> Products { get; init; } = [];
}
