using MediatR;
using Asp.Versioning;
using Edition.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.Wallets.Queries;
using Edition.Application.Features.Wallets.Commands;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// User wallet management
/// </summary>
[Authorize]
[ApiVersion("1")]
[ControllerName("wallet")]
[ApiControllerCategory(ApiControllerCategory.Wallet)]
public class WalletController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("my-wallets")]
    public async Task<ApiResult<List<GetMyWalletResponse>>> GetMyWallets()
        => await mediator.Send(new GetMyWalletsRequest());

    [HttpPost("get")]
    public async Task<ApiResult<GetMyWalletResponse?>> Get(GetMyWalletRequest request)
        => await mediator.Send(request);

    [HttpPost("transactions")]
    public async Task<ApiResult<PagedResult<GetWalletTransactionResponse>>> GetTransactions(GetMyWalletTransactionsRequest request)
        => await mediator.Send(request);

    [HttpPost("charge")]
    public async Task<ApiResult<ChargeWalletResponse>> Charge(ChargeWalletRequest request)
        => await mediator.Send(request);

    [HttpPost("charge/verify")]
    public async Task<ApiResult<VerifyWalletChargeResponse>> VerifyCharge(VerifyWalletChargeRequest request)
        => await mediator.Send(request);
}
