using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetUserAddressHandler 
    (IUserAddressRepository userAddressRepository, IUserAddressMapperService mapper)
    : IRequestHandler<GetUserAddressRequest, OperationResult<GetUserAddressResponse>>
{
    public async Task<OperationResult<GetUserAddressResponse>> Handle(GetUserAddressRequest request, CancellationToken cancellationToken)
    {
        var userAddress = await userAddressRepository.GetAsNoTrackingAsync(request.Id);
        if (userAddress is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(userAddress);
        return result;
    }
}