using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.DeliveryOptions.Queries;
using Edition.Application.Features.DeliveryOptions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Delivery Options For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("delivery-option")]
[ApiControllerCategory(ApiControllerCategory.Delivery)]
[ControllerInfo(PermissionType.ManageDeliveryOption, PermissionType.ManageDeliveryTypeGroup)]
public class DeliveryOptionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListDeliveryOption)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllDeliveryOptionResponse>>> GetAll(GetAllDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetDeliveryOptionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetDeliveryOptionResponse?>> Get(GetDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateDeliveryOption)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateDeliveryOptionResponse>> Create(CreateDeliveryOptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateDeliveryOption)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateDeliveryOptionRequest request)
        => await mediator.Send(request);
}
