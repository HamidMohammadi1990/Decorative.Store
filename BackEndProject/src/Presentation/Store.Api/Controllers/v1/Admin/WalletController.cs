using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Wallets.Queries;
using Edition.Application.Features.Wallets.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Wallets For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("wallet")]
[ApiControllerCategory(ApiControllerCategory.Wallet)]
[ControllerInfo(PermissionType.ManageWallet, PermissionType.ManageFinancialGroup)]
public class WalletController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListWallet)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllWalletResponse>>> GetAll(GetAllWalletRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetWalletById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetWalletResponse?>> Get(GetWalletRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListWalletTransaction)]
    [HttpPost("transactions")]
    public async Task<ApiResult<PagedResult<GetWalletTransactionResponse>>> GetTransactions(GetAllWalletTransactionsRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateWallet)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateWalletResponse>> Create(CreateWalletRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateWallet)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateWalletRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateWalletStatus)]
    [HttpPut("status")]
    public async Task<ApiResult<OperationResult>> UpdateStatus(UpdateWalletStatusRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.AdminChargeWallet)]
    [HttpPost("charge")]
    public async Task<ApiResult<AdminChargeWalletResponse>> Charge(AdminChargeWalletRequest request)
        => await mediator.Send(request);
}
