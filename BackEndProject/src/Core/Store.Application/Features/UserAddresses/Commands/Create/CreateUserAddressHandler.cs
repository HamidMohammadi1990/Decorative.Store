using Store.Common.Extensions;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserAddresses.Commands;

public class CreateUserAddressHandler
    (IUnitOfWork uow, IUserAddressRepository userAddressRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateUserAddressRequest, OperationResult<CreateUserAddressResponse>>
{
    public async Task<OperationResult<CreateUserAddressResponse>> Handle(CreateUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userAddress = UserAddress.Create(request.Title, currentUser.UserId, request.Address, request.PostalCode,
                                      request.CityId, request.RecipientFirstName, request.RecipientLastName, request.PhoneNumber);

        userAddressRepository.Add(userAddress);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateUserAddressResponse>();

        return new CreateUserAddressResponse { Id = userAddress.Id };
    }
}