namespace Edition.Application.Features.Catalog.Queries;

public record GetNewestCatalogProductsResponse
{
    public List<CatalogListingProductResponse> Products { get; init; } = [];
}
