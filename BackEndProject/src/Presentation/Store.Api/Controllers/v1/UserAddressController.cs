using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.UserAddresses.Queries;
using Edition.Application.Features.UserAddresses.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management User Addresses
/// </summary>
[Authorize]
[ApiVersion("1")]
[ControllerName("user-address")]
[ApiControllerCategory(ApiControllerCategory.Users)]
public class UserAddressController
    (ISender mediator)
    : BaseApiController
{   
    [HttpPost("my")]
    public async Task<ApiResult<PagedResult<GetUserAddressesResponse>>> UserAddresses(GetUserAddressesRequest request)
        => await mediator.Send(request);

    [HttpPost("create")]
    public async Task<ApiResult<CreateUserAddressResponse>> Create(CreateUserAddressRequest request)
        => await mediator.Send(request);

    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateUserAddressRequest request)
        => await mediator.Send(request);

    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteUserAddressRequest request)
        => await mediator.Send(request);
}