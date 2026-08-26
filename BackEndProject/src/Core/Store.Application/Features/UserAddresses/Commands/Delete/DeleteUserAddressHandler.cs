using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Commands;

public class DeleteUserAddressHandler 
    (IUserAddressRepository userAddressRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteUserAddressRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userAddress = await userAddressRepository.FindAsync(request.Id);
        if (userAddress is null)
            return ErrorModel.Create("InvalidId");

        var userId = userAddress.UserId;
        var wasDefault = userAddress.IsDefault;

        userAddressRepository.Remove(userAddress);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        if (wasDefault)
        {
            await userAddressRepository.PromoteNextDefaultAsync(
                userId,
                cancellationToken: cancellationToken);
            saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
            if (!saveChangesResult.IsSuccess)
                return saveChangesResult;
        }

        return OperationResult.Success();
    }
}
