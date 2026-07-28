using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Orders.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Orders For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("order")]
[ApiControllerCategory(ApiControllerCategory.Orders)]
[ControllerInfo(PermissionType.ManageOrder, PermissionType.ManageOrderGroup)]
public class OrderController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListOrder)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllOrderResponse>>> GetAll(GetAllOrderRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetOrderById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetOrderResponse?>> Get(GetOrderRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListOrderByStatus)]
    [HttpPost("by-status")]
    public async Task<ApiResult<List<GetOrderByStatusResponse>>> GetByStatus(GetOrderByStatusRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetOrderDetail)]
    [HttpPost("detail")]
    public async Task<ApiResult<GetOrderDetailResponse?>> GetDetail(GetOrderDetailRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetOrderStatusSummary)]
    [HttpGet("status-summary")]
    public async Task<ApiResult<List<GetStatusSummaryOrderResponse>>> StatusSummary()
        => await mediator.Send(new GetStatusSummaryOrderRequest());
}
