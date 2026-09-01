using Edition.Application.Features.Catalog.Queries;
using Edition.Application.Features.Catalog.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Catalog listing pages
/// </summary>
[ApiVersion("1")]
[ControllerName("catalog")]
[ApiControllerCategory(ApiControllerCategory.Catalog)]
public class CatalogController(ISender mediator) : BaseApiController
{
    [HttpGet("listing")]
    public async Task<ApiResult<GetCatalogListingResponse>> Listing(
        [FromQuery] string path = "",
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] bool? inStock = null,
        [FromQuery] bool? onSale = null,
        [FromQuery] bool? isNew = null,
        [FromQuery] double? minRating = null,
        [FromQuery] string? sort = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery(Name = "price")] string[]? price = null)
    {
        var attributeFilters = ParseAttributeFilters(Request.Query);

        return await mediator.Send(new GetCatalogListingRequest
        {
            Path = path,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            InStock = inStock,
            OnSale = onSale,
            IsNew = isNew,
            MinRating = minRating,
            Sort = sort,
            Page = page,
            PageSize = pageSize,
            PriceBuckets = price?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? [],
            AttributeFilters = attributeFilters,
        });
    }

    [HttpGet("search")]
    public async Task<ApiResult<GetCatalogSearchResponse>> Search(
        [FromQuery] string q,
        [FromQuery] int limit = 8)
        => await mediator.Send(new GetCatalogSearchRequest(q, limit));

    [HttpGet("product")]
    public async Task<ApiResult<GetCatalogProductResponse>> Product([FromQuery] string slug)
        => await mediator.Send(new GetCatalogProductRequest(slug));

    [HttpGet("product/related")]
    public async Task<ApiResult<GetRelatedCatalogProductsResponse>> Related(
        [FromQuery] string slug,
        [FromQuery] int limit = 4)
        => await mediator.Send(new GetRelatedCatalogProductsRequest(slug, limit));

    [HttpGet("newest")]
    public async Task<ApiResult<GetNewestCatalogProductsResponse>> Newest([FromQuery] int limit = 4)
        => await mediator.Send(new GetNewestCatalogProductsRequest(limit));

    [HttpGet("featured-collections")]
    public async Task<ApiResult<GetFeaturedCatalogCollectionsResponse>> FeaturedCollections()
        => await mediator.Send(new GetFeaturedCatalogCollectionsRequest());

    private static Dictionary<string, List<string>> ParseAttributeFilters(IQueryCollection query)
    {
        var filters = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in query)
        {
            if (CatalogListingFilterProcessor.IsReservedQueryKey(entry.Key))
                continue;

            var values = entry.Value
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.Trim())
                .ToList();

            if (values.Count == 0)
                continue;

            filters[entry.Key] = values;
        }

        return filters;
    }
}
