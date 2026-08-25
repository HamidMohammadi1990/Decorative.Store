using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.Discounts.Queries;
using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User-facing discount codes
/// </summary>
[Authorize]
[ApiVersion("1")]
[ControllerName("discount")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class DiscountController(ISender mediator) : BaseApiController
{
    [HttpGet("my")]
    public async Task<ApiResult<GetMyAvailableDiscountsResponse>> My()
        => await mediator.Send(new GetMyAvailableDiscountsRequest());
}
