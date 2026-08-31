using Edition.Application.Common.Directories;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetFeaturedCatalogCollectionsHandler(IProductRepository productRepository)
    : IRequestHandler<GetFeaturedCatalogCollectionsRequest, OperationResult<GetFeaturedCatalogCollectionsResponse>>
{
    public async Task<OperationResult<GetFeaturedCatalogCollectionsResponse>> Handle(
        GetFeaturedCatalogCollectionsRequest request,
        CancellationToken cancellationToken)
    {
        var newArrivals = await productRepository.GetNewArrivalsCatalogProductsAsync(1, cancellationToken);
        var inStock = await productRepository.GetInStockCatalogProductsAsync(1, cancellationToken);
        var bestSellers = await productRepository.GetBestSellingCatalogProductsAsync(1, cancellationToken);

        var collections = new List<FeaturedCatalogCollectionResponse>();

        AddCollection(collections, "new-arrivals", "/new", newArrivals.FirstOrDefault());
        AddCollection(collections, "in-stock", "/in-stock", inStock.FirstOrDefault());
        AddCollection(collections, "best-sellers", "/best-sellers", bestSellers.FirstOrDefault());

        return new GetFeaturedCatalogCollectionsResponse
        {
            Collections = collections,
        };
    }

    private static void AddCollection(
        ICollection<FeaturedCatalogCollectionResponse> collections,
        string id,
        string href,
        Store.Domain.Dtos.Catalog.CatalogListingProductDto? product)
    {
        if (product is null)
            return;

        collections.Add(new FeaturedCatalogCollectionResponse
        {
            Id = id,
            Href = href,
            ImageUrl = string.IsNullOrWhiteSpace(product.ImageFileName)
                ? string.Empty
                : ProductDirectory.GetImageUrl(product.ImageFileName),
            ImageAlt = product.ImageAlt,
        });
    }
}
