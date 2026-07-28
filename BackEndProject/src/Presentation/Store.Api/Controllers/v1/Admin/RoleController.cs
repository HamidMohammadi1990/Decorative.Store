using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Roles.Queries;
using Edition.Application.Features.Roles.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Roles For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("role")]
[ApiControllerCategory(ApiControllerCategory.AccessControl)]
[ControllerInfo(PermissionType.ManageRole, PermissionType.ManageRoleGroup)]
public class RoleController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListRole)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllRoleResponse>>> GetAll(GetAllRoleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetRoleById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetRoleResponse?>> Get(GetRoleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateRole)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateRoleResponse>> Create(CreateRoleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateRole)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateRoleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteRole)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteRoleRequest request)
        => await mediator.Send(request);
}
