using Asp.Versioning;
using Edition.Application.Features.ProductProperties.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Properties
/// </summary>
[ApiVersion("1")]
[ControllerName("product-property")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductPropertyController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductPropertyResponse>>> Search(SearchProductPropertyRequest request)
        => await mediator.Send(request);
}
