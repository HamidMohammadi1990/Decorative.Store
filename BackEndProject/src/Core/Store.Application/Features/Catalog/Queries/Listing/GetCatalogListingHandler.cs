using Edition.Application.Common.Directories;
using Edition.Application.Contracts.Localization;
using Edition.Application.Features.Catalog.Services;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Catalog.Queries;

public class GetCatalogListingHandler(
    IProductRepository productRepository,
    ILanguageRegistry languageRegistry,
    ICurrentLanguageContext languageContext)
    : IRequestHandler<GetCatalogListingRequest, OperationResult<GetCatalogListingResponse>>
{
    public async Task<OperationResult<GetCatalogListingResponse>> Handle(
        GetCatalogListingRequest request,
        CancellationToken cancellationToken)
    {
        var listing = await productRepository.GetCatalogListingByPathAsync(request.Path, cancellationToken);
        var isFa = await ResolveIsFaAsync(cancellationToken);
        var processed = CatalogListingFilterProcessor.Apply(listing, request, isFa);

        return new GetCatalogListingResponse
        {
            Title = processed.Title,
            PathNotFound = processed.PathNotFound,
            Breadcrumbs = processed.Breadcrumbs
                .Select(item => new CatalogListingBreadcrumbResponse
                {
                    Label = item.Label,
                    Href = item.Href
                })
                .ToList(),
            Products = processed.Products
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
                    Facets = product.Facets,
                    ReviewCount = product.ReviewCount,
                    AverageRating = product.AverageRating,
                    PurchaseCount = product.PurchaseCount,
                })
                .ToList(),
            TotalCount = processed.TotalCount,
            Page = processed.Page,
            PageSize = processed.PageSize,
            FacetGroups = processed.FacetGroups
                .Select(group => new CatalogListingFacetGroupResponse
                {
                    Id = group.Id,
                    Label = group.Label,
                    Type = group.Type,
                    Options = group.Options
                        .Select(option => new CatalogListingFacetOptionResponse
                        {
                            Value = option.Value,
                            Label = option.Label,
                            Count = option.Count,
                            Swatch = option.Swatch,
                        })
                        .ToList(),
                    Range = group.Range is null
                        ? null
                        : new CatalogListingPriceRangeResponse
                        {
                            Min = group.Range.Min,
                            Max = group.Range.Max,
                            Step = group.Range.Step,
                            SelectedMin = group.Range.SelectedMin,
                            SelectedMax = group.Range.SelectedMax,
                        },
                })
                .ToList(),
        };
    }

    private async Task<bool> ResolveIsFaAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var language = await languageRegistry.GetByIdAsync(languageId, cancellationToken)
                       ?? defaultLanguage;
        return (language.Code ?? "en-US").StartsWith("fa", StringComparison.OrdinalIgnoreCase);
    }
}
