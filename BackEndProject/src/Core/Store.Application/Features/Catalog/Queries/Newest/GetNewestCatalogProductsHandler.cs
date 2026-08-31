using Edition.Application.Common.Directories;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetNewestCatalogProductsHandler(IProductRepository productRepository)
    : IRequestHandler<GetNewestCatalogProductsRequest, OperationResult<GetNewestCatalogProductsResponse>>
{
    public async Task<OperationResult<GetNewestCatalogProductsResponse>> Handle(
        GetNewestCatalogProductsRequest request,
        CancellationToken cancellationToken)
    {
        var products = await productRepository.GetNewestCatalogProductsAsync(
            request.Limit,
            cancellationToken);

        return new GetNewestCatalogProductsResponse
        {
            Products = products
                .Select(product => new CatalogListingProductResponse
                {
                    Id = product.Id,
                    Title = product.Title,
                    Slug = product.Slug,
                    ImageUrl = string.IsNullOrWhiteSpace(product.ImageFileName)
                        ? string.Empty
                        : ProductDirectory.GetImageUrl(product.ImageFileName),
                    ImageAlt = product.ImageAlt,
                    Price = product.Price,
                    CurrencyCode = product.CurrencyCode,
                    CompareAtPrice = product.CompareAtPrice,
                    InStock = product.InStock,
                    OnSale = product.OnSale,
                    IsNew = product.IsNew,
                    CategorySlug = product.CategorySlug,
                    SubCategorySlug = product.SubCategorySlug,
                })
                .ToList(),
        };
    }
}
