using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Wallets.Queries;

public class GetMyWalletTransactionsHandler
    (IWalletTransactionRepository walletTransactionRepository, IWalletMapperService mapper, ICurrentUserContext currentUser, IWalletRepository walletRepository)
    : IRequestHandler<GetMyWalletTransactionsRequest, OperationResult<PagedResult<GetWalletTransactionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetWalletTransactionResponse>>> Handle(
        GetMyWalletTransactionsRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        if (request.WalletId.HasValue)
        {
            var wallet = await walletRepository.FindByUserIdAsync(userId, request.WalletId.Value, cancellationToken);
            if (wallet is null)
                return ErrorModel.Create("WalletNotFound");
        }

        var requestModel = mapper.Map(request);
        var transactions = await walletTransactionRepository.GetByUserAsync(userId, requestModel);
        return mapper.MapTransactions(transactions);
    }
}