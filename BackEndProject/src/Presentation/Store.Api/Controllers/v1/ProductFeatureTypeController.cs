using Asp.Versioning;
using Edition.Application.Features.ProductFeatureTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Feature Types
/// </summary>
[ApiVersion("1")]
[ControllerName("product-feature-type")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductFeatureTypeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductFeatureTypeResponse>>> Search(SearchProductFeatureTypeRequest request)
        => await mediator.Send(request);
}
