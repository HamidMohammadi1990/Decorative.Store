using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Wallets.Commands;

public class CreateWalletHandler
    (IUnitOfWork uow, IWalletRepository walletRepository, IUserRepository userRepository)
    : IRequestHandler<CreateWalletRequest, OperationResult<CreateWalletResponse>>
{
    public async Task<OperationResult<CreateWalletResponse>> Handle(CreateWalletRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindAsync(request.UserId, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        if (request.IsDefault)
            await walletRepository.ClearDefaultForUserAsync(request.UserId, exceptWalletId: 0, cancellationToken);

        var wallet = Wallet.Create(request.UserId, request.Title.Trim(), request.IsDefault);
        walletRepository.Add(wallet);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<CreateWalletResponse>();

        return new CreateWalletResponse { Id = wallet.Id };
    }
}
