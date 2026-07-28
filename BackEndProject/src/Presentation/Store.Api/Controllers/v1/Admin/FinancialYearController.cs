using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.FinancialYears.Queries;
using Edition.Application.Features.FinancialYears.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Financial Years For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("financial-year")]
[ApiControllerCategory(ApiControllerCategory.Financial)]
[ControllerInfo(PermissionType.ManageFinancialYear, PermissionType.ManageFinancialGroup)]
public class FinancialYearController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListFinancialYear)]
    [HttpGet("get-all")]
    public async Task<ApiResult<PagedResult<GetAllFinancialYearResponse>>> GetAll(GetAllFinancialYearRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetFinancialYearById)]
    [HttpGet("get")]
    public async Task<ApiResult<GetFinancialYearResponse?>> Get(GetFinancialYearRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateFinancialYear)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateFinancialYearResponse>> Create(CreateFinancialYearRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateFinancialYear)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateFinancialYearRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteFinancialYear)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteFinancialYearRequest request)
        => await mediator.Send(request);
}
