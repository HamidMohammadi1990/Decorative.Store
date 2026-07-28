using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Wallets.Commands;

public class UpdateWalletStatusHandler
    (IUnitOfWork uow, IWalletRepository walletRepository)
    : IRequestHandler<UpdateWalletStatusRequest, OperationResult<OperationResult>>
{
    public async Task<OperationResult<OperationResult>> Handle(UpdateWalletStatusRequest request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.FindAsync(request.Id, cancellationToken);
        if (wallet is null)
            return ErrorModel.Create("WalletNotFound");

        wallet.ChangeStatus(request.Status);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess
            ? OperationResult.Success()
            : saveResult.ToGenericFailure<OperationResult>();
    }
}
