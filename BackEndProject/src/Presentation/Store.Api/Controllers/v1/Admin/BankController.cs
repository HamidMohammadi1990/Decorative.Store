using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Banks.Queries;
using Edition.Application.Features.Banks.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Banks For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("bank")]
[ApiControllerCategory(ApiControllerCategory.Financial)]
[ControllerInfo(PermissionType.ManageBank, PermissionType.ManageFinancialGroup)]
public class BankController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBank)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBankResponse>>> GetAll(GetAllBankRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBankById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBankResponse?>> Get(GetBankRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateBank)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateBankResponse>> Create(CreateBankRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBank)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateBankRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteBank)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteBankRequest request)
        => await mediator.Send(request);
}
