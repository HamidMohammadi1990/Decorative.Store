using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Catalog.Queries;
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
    public async Task<ApiResult<GetCatalogListingResponse>> Listing([FromQuery] string path = "")
        => await mediator.Send(new GetCatalogListingRequest(path));

    [HttpGet("product")]
    public async Task<ApiResult<GetCatalogProductResponse>> Product([FromQuery] string slug)
        => await mediator.Send(new GetCatalogProductRequest(slug));

    [HttpGet("product/related")]
    public async Task<ApiResult<GetRelatedCatalogProductsResponse>> Related(
        [FromQuery] string slug,
        [FromQuery] int limit = 4)
        => await mediator.Send(new GetRelatedCatalogProductsRequest(slug, limit));
}
