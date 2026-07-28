using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Discounts.Queries;
using Edition.Application.Features.Discounts.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Discounts For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("discount")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageDiscount, PermissionType.ManageProductGroup)]
public class DiscountController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListDiscount)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllDiscountResponse>>> GetAll(GetAllDiscountRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetDiscountById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetDiscountResponse?>> Get(GetDiscountRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateDiscount)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateDiscountResponse>> Create(CreateDiscountRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateDiscount)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateDiscountRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteDiscount)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteDiscountRequest request)
        => await mediator.Send(request);
}