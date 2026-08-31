using Edition.Application.Common.Directories;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogListingHandler(IProductRepository productRepository)
    : IRequestHandler<GetCatalogListingRequest, OperationResult<GetCatalogListingResponse>>
{
    public async Task<OperationResult<GetCatalogListingResponse>> Handle(
        GetCatalogListingRequest request,
        CancellationToken cancellationToken)
    {
        var listing = await productRepository.GetCatalogListingByPathAsync(request.Path, cancellationToken);

        return new GetCatalogListingResponse
        {
            Title = listing.Title,
            PathNotFound = listing.PathNotFound,
            Breadcrumbs = listing.Breadcrumbs
                .Select(item => new CatalogListingBreadcrumbResponse
                {
                    Label = item.Label,
                    Href = item.Href
                })
                .ToList(),
            Products = listing.Products
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
                    SubCategorySlug = product.SubCategorySlug
                })
                .ToList()
        };
    }
}
