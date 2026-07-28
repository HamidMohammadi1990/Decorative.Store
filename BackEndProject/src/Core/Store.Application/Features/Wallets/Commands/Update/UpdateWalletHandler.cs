using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Commands;

public class UpdateWalletHandler
    (IUnitOfWork uow, IWalletRepository walletRepository)
    : IRequestHandler<UpdateWalletRequest, OperationResult<OperationResult>>
{
    public async Task<OperationResult<OperationResult>> Handle(UpdateWalletRequest request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.FindAsync(request.Id, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        if (request.IsDefault && wallet.UserId.HasValue)
            await walletRepository.ClearDefaultForUserAsync(wallet.UserId.Value, wallet.Id, cancellationToken);

        wallet.Update(request.Title.Trim(), request.IsDefault);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess
            ? OperationResult.Success()
            : saveResult.ToGenericFailure<OperationResult>();
    }
}
