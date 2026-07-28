using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.PropertyItems.Queries;
using Edition.Application.Features.PropertyItems.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Property Items For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("property-item")]
[ApiControllerCategory(ApiControllerCategory.Property)]
[ControllerInfo(PermissionType.ManagePropertyItem, PermissionType.ManagePropertyGroup)]
public class PropertyItemController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPropertyItem)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPropertyItemResponse>>> GetAll(GetAllPropertyItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPropertyItemById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPropertyItemResponse?>> Get(GetPropertyItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreatePropertyItem)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePropertyItemResponse>> Create(CreatePropertyItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePropertyItem)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePropertyItemRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePropertyItem)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePropertyItemRequest request)
        => await mediator.Send(request);
}
