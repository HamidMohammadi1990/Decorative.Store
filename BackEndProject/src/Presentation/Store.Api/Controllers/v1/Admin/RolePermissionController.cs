using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.RolePermissions.Queries;
using Edition.Application.Features.RolePermissions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Role Permissions For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("role-permission")]
[ApiControllerCategory(ApiControllerCategory.AccessControl)]
[ControllerInfo(PermissionType.ManageRolePermission, PermissionType.ManageRolePermissionGroup)]
public class RolePermissionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListRolePermission)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllRolePermissionResponse>>> GetAll(GetAllRolePermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetRolePermissionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetRolePermissionResponse?>> Get(GetRolePermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.AssignRolePermission)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateRolePermissionResponse>> Create(CreateRolePermissionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteRolePermission)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteRolePermissionRequest request)
        => await mediator.Send(request);
}
