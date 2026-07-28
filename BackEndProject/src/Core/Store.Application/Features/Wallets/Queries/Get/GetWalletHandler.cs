using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Queries;

public class GetWalletHandler
    (IWalletRepository walletRepository, IWalletMapperService mapper)
    : IRequestHandler<GetWalletRequest, OperationResult<GetWalletResponse?>>
{
    public async Task<OperationResult<GetWalletResponse?>> Handle(GetWalletRequest request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        return mapper.Map(wallet);
    }
}