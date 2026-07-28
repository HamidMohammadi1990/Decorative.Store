using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Queries;

public class GetMyWalletsHandler
    (IWalletRepository walletRepository, IWalletMapperService mapper, ICurrentUserContext currentUser, IUnitOfWork uow)
    : IRequestHandler<GetMyWalletsRequest, OperationResult<List<GetMyWalletResponse>>>
{
    public async Task<OperationResult<List<GetMyWalletResponse>>> Handle(GetMyWalletsRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var wallets = await walletRepository.GetByUserIdAsync(userId, cancellationToken);

        if (wallets.Count == 0)
        {
            var defaultWallet = Wallet.Create(userId, "کیف پول اصلی", isDefault: true);
            walletRepository.Add(defaultWallet);

            var saveResult = await uow.SaveChangesAsync(cancellationToken);
            if (!saveResult.IsSuccess)
                return saveResult.ToGenericFailure<List<GetMyWalletResponse>>();

            wallets = [defaultWallet];
        }

        return mapper.MapToMyWallets(wallets);
    }
}
