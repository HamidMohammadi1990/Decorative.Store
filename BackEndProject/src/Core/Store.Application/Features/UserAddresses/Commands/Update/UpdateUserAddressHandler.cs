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

        userAddress.Update(
            request.Title,
            request.IsActive,
            request.Address,
            request.PostalCode,
            request.CityId,
            request.RecipientFirstName,
            request.RecipientLastName,
            request.PhoneNumber);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}