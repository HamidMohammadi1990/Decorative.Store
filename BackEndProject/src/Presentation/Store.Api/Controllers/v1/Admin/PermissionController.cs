using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Permissions.Queries;
using Edition.Application.Features.Permissions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Permissions For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("permission")]
[ApiControllerCategory(ApiControllerCategory.AccessControl)]
[ControllerInfo(PermissionType.ManagePermission, PermissionType.ManagePermissionGroup)]
public class PermissionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListPermission)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPermissionResponse>>> GetAll(GetAllPermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPermissionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPermissionResponse?>> Get(GetPermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CheckPermission)]
    [HttpPost("has-permission")]
    public async Task<ApiResult<HasPermissionResponse>> HasPermission(HasPermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateManagedPermission)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePermissionResponse>> Create(CreatePermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdatePermission)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeletePermission)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePermissionRequest request)
        => await mediator.Send(request);
}
