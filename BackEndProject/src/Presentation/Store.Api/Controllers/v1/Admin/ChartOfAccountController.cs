using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.ChartOfAccounts.Queries;
using Edition.Application.Features.ChartOfAccounts.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Chart Of Accounts For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("chart-of-account")]
[ApiControllerCategory(ApiControllerCategory.Financial)]
[ControllerInfo(PermissionType.ManageChartOfAccount, PermissionType.ManageFinancialGroup)]
public class ChartOfAccountController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListChartOfAccount)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllChartOfAccountResponse>>> GetAll(GetAllChartOfAccountRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetChartOfAccountById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetChartOfAccountResponse?>> Get(GetChartOfAccountRequest request)
        => await mediator.Send(request);

    [Authorize]
    [ActionInfo(PermissionType.CreateChartOfAccount)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateChartOfAccountResponse>> Create(CreateChartOfAccountRequest request)
        => await mediator.Send(request);

    [Authorize]
    [ActionInfo(PermissionType.UpdateChartOfAccount)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateChartOfAccountRequest request)
        => await mediator.Send(request);

    [Authorize]
    [ActionInfo(PermissionType.DeleteChartOfAccount)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteChartOfAccountRequest request)
        => await mediator.Send(request);
}
