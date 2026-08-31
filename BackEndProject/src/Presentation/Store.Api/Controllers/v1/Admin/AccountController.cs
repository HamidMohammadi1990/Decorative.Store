using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Users.Queries;
using Edition.Application.Features.Users.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Common.Models;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management User Account For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("account")]
[ApiControllerCategory(ApiControllerCategory.Authentication)]
[ControllerInfo(PermissionType.ManageUsers, PermissionType.ManageUsersGroup)]
public class AccountController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListUser)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllUserResponse>>> GetAll(GetAllUserRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetUserById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetUserResponse?>> Get(GetUserRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateUser)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateUserResponse>> Create(CreateUserRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateUser)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateUserRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteUser)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteUserRequest request)
        => await mediator.Send(request);
}
