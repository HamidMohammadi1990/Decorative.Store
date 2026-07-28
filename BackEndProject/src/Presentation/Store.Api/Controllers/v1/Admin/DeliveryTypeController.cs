using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.DeliveryTypes.Queries;
using Edition.Application.Features.DeliveryTypes.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Delivery Types For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("delivery-type")]
[ApiControllerCategory(ApiControllerCategory.Delivery)]
[ControllerInfo(PermissionType.ManageDeliveryType, PermissionType.ManageDeliveryTypeGroup)]
public class DeliveryTypeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListDeliveryType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllDeliveryTypeResponse>>> GetAll(GetAllDeliveryTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetDeliveryTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetDeliveryTypeResponse?>> Get(GetDeliveryTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateDeliveryType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateDeliveryTypeResponse>> Create(CreateDeliveryTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateDeliveryType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateDeliveryTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteDeliveryType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteDeliveryTypeRequest request)
        => await mediator.Send(request);
}
