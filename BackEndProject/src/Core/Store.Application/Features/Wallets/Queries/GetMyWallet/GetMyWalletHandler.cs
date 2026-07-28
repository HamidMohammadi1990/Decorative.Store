using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Queries;

public class GetMyWalletHandler
    (IWalletRepository walletRepository, IWalletMapperService mapper, ICurrentUserContext currentUser)
    : IRequestHandler<GetMyWalletRequest, OperationResult<GetMyWalletResponse?>>
{
    public async Task<OperationResult<GetMyWalletResponse?>> Handle(GetMyWalletRequest request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.FindByUserIdAsync(currentUser.UserId, request.Id, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        return mapper.MapToMyWallet(wallet);
    }
}
