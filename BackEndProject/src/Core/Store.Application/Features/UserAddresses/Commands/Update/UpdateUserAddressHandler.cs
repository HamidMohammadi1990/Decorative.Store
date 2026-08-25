using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Commands;

public class UpdateUserAddressHandler
    (IUserAddressRepository userAddressRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateUserAddressRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userAddress = await userAddressRepository.FindAsync(request.Id);
        if (userAddress is null)
            return ErrorModel.Create("InvalidId");

        var wasDefault = userAddress.IsDefault;
        var makeDefault = request.IsDefault;

        if (makeDefault)
            await userAddressRepository.ClearDefaultForUserAsync(userAddress.UserId, userAddress.Id, cancellationToken);

        userAddress.Update(
            request.Title,
            request.IsActive,
            request.Address,
            request.Apartment,
            request.PostalCode,
            request.CityId,
            request.RecipientFirstName,
            request.RecipientLastName,
            request.PhoneNumber,
            makeDefault);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        if (!makeDefault && wasDefault)
        {
            await userAddressRepository.PromoteNextDefaultAsync(
                userAddress.UserId,
                exceptAddressId: userAddress.Id,
                cancellationToken);
            saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
            if (!saveChangesResult.IsSuccess)
                return saveChangesResult;
        }

        return OperationResult.Success();
    }
}
