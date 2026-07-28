using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.DeliveryTypes.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Delivery Types
/// </summary>
[ApiVersion("1")]
[ControllerName("delivery-type")]
[ApiControllerCategory(ApiControllerCategory.Delivery)]
public class DeliveryTypeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchDeliveryTypeResponse>>> Search(SearchDeliveryTypeRequest request)
        => await mediator.Send(request);
}