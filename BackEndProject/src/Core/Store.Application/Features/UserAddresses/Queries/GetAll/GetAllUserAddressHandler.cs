using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetAllUserAddressHandler
    (IUserAddressRepository userAddressRepository, IUserAddressMapperService mapper)
    : IRequestHandler<GetAllUserAddressRequest, OperationResult<PagedResult<GetAllUserAddressResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllUserAddressResponse>>> Handle(GetAllUserAddressRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var addresses = await userAddressRepository.GetAllAsync(requestModel);
        var result = mapper.Map(addresses);
        return result;
    }
}