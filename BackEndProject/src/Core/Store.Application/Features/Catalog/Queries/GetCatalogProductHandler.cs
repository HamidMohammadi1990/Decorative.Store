using Edition.Application.Common.Directories;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogProductHandler(IProductRepository productRepository)
    : IRequestHandler<GetCatalogProductRequest, OperationResult<GetCatalogProductResponse>>
{
    public async Task<OperationResult<GetCatalogProductResponse>> Handle(
        GetCatalogProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.GetCatalogProductBySlugAsync(request.Slug, cancellationToken);

        if (product is null || product.NotFound || product.Product is null)
        {
            return new GetCatalogProductResponse { NotFound = true };
        }

        var summary = product.Product;

        return new GetCatalogProductResponse
        {
            NotFound = false,
            Id = summary.Id,
            Title = summary.Title,
            Slug = summary.Slug,
            ImageUrl = string.IsNullOrWhiteSpace(summary.ImageFileName)
                ? string.Empty
                : ProductDirectory.GetImageUrl(summary.ImageFileName),
            ImageAlt = summary.ImageAlt,
            Price = summary.Price,
            CurrencyCode = summary.CurrencyCode,
            CompareAtPrice = summary.CompareAtPrice,
            InStock = summary.InStock,
            OnSale = summary.OnSale,
            IsNew = summary.IsNew,
            CategorySlug = summary.CategorySlug,
            SubCategorySlug = summary.SubCategorySlug,
            Description = product.Description,
            LongDescriptions = product.LongDescriptions,
            Images = product.Images
                .Select(image => new CatalogProductImageResponse
                {
                    Url = string.IsNullOrWhiteSpace(image.Url)
                        ? string.Empty
                        : ProductDirectory.GetImageUrl(image.Url),
                    Alt = image.Alt
                })
                .ToList(),
            Features = product.Features
                .Select(feature => new CatalogProductFeatureResponse
                {
                    Label = feature.Label,
                    Value = feature.Value,
                    GroupTitle = feature.GroupTitle
                })
                .ToList()
        };
    }
}
