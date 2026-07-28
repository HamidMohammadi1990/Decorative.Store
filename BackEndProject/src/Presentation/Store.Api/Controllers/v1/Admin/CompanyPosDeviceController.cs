using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyPosDevices.Queries;
using Edition.Application.Features.CompanyPosDevices.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Company Pos Device For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company-pos-device")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompanyPosDevice, PermissionType.ManageCompanyGroup)]
public class CompanyPosDeviceController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompanyPosDevice)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyPosDeviceResponse>>> GetAll(GetAllCompanyPosDeviceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyPosDeviceById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyPosDeviceResponse?>> Get(GetCompanyPosDeviceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateCompanyPosDevice)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyPosDeviceResponse>> Create(CreateCompanyPosDeviceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateCompanyPosDevice)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCompanyPosDeviceRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteCompanyPosDevice)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteCompanyPosDeviceRequest request)
        => await mediator.Send(request);
}
