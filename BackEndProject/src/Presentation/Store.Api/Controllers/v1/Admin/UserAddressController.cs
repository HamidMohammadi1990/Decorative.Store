using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.UserAddresses.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management User Address For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("user-address")]
[ApiControllerCategory(ApiControllerCategory.Users)]
[ControllerInfo(PermissionType.ManageUserAddress, PermissionType.ManageUserAddressGroup)]
public class UserAddressController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListUserAddress)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllUserAddressResponse>>> GetAll(GetAllUserAddressRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetUserAddressById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetUserAddressResponse?>> Get(GetUserAddressRequest request)
        => await mediator.Send(request);
}
