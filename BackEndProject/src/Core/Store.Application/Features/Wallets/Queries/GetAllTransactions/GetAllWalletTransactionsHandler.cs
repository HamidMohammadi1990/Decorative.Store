using Edition.Application.Contracts.Mapping;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Wallets.Queries;

public class GetAllWalletTransactionsHandler
    (IWalletTransactionRepository walletTransactionRepository, IWalletMapperService mapper)
    : IRequestHandler<GetAllWalletTransactionsRequest, OperationResult<PagedResult<GetWalletTransactionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetWalletTransactionResponse>>> Handle(
        GetAllWalletTransactionsRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var transactions = await walletTransactionRepository.GetAllAsync(requestModel);
        return mapper.MapTransactions(transactions);
    }
}