using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.DeliveryOptions.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Delivery Options
/// </summary>
[ApiVersion("1")]
[ControllerName("delivery-option")]
[ApiControllerCategory(ApiControllerCategory.Delivery)]
public class DeliveryOptionController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchDeliveryOptionResponse>>> Search(SearchDeliveryOptionRequest request)
        => await mediator.Send(request);
}