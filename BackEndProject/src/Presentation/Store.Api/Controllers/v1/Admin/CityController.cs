using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Cities.Queries;
using Edition.Application.Features.Cities.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Cities For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("city")]
[ApiControllerCategory(ApiControllerCategory.Location)]
[ControllerInfo(PermissionType.ManageCity, PermissionType.ManageLocationGroup)]
public class CityController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCity)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCityResponse>>> GetAll(GetAllCityRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCityById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCityResponse>> Get(GetCityRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateCity)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCityResponse>> Create(CreateCityRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateCity)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCityRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteCity)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCityRequest request)
        => await mediator.Send(request);
}