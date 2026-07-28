using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Properties.Queries;
using Edition.Application.Features.Properties.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Properties For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("property")]
[ApiControllerCategory(ApiControllerCategory.Property)]
[ControllerInfo(PermissionType.ManageProperty, PermissionType.ManagePropertyGroup)]
public class PropertyController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProperty)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllPropertyResponse>>> GetAll(GetAllPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetPropertyById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetPropertyResponse?>> Get(GetPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProperty)]
    [HttpPost("create")]
    public async Task<ApiResult<CreatePropertyResponse>> Create(CreatePropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProperty)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdatePropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProperty)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeletePropertyRequest request)
        => await mediator.Send(request);
}
