using Edition.Application.Common.Directories;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogSearchHandler(IProductRepository productRepository)
    : IRequestHandler<GetCatalogSearchRequest, OperationResult<GetCatalogSearchResponse>>
{
    public async Task<OperationResult<GetCatalogSearchResponse>> Handle(
        GetCatalogSearchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await productRepository.SearchCatalogAsync(
            request.Query,
            request.Limit,
            cancellationToken);

        return new GetCatalogSearchResponse
        {
            Categories = result.Categories
                .Select(category => new CatalogSearchCategoryResponse
                {
                    Title = category.Title,
                    Slug = category.Slug
                })
                .ToList(),
            SubCategories = result.SubCategories
                .Select(subCategory => new CatalogSearchSubCategoryResponse
                {
                    Title = subCategory.Title,
                    Slug = subCategory.Slug,
                    CategoryTitle = subCategory.CategoryTitle,
                    CategorySlug = subCategory.CategorySlug
                })
                .ToList(),
            Products = result.Products
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
