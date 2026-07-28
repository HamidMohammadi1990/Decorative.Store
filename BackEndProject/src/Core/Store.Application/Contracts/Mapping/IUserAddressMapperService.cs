using Edition.Application.Features.UserAddresses.Queries;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IUserAddressMapperService : IMapper
{
    GetUserAddressResponse Map(UserAddress userAddress);
    PagedResult<GetUserAddressesResponse> Map(PagedResult<GetUserAddressDto> model);
    GetAllUserAddressRequestDto Map(GetAllUserAddressRequest model);
    PagedResult<GetAllUserAddressResponse> Map(PagedResult<GetAllUserAddressDto> model);	
    GetUserAddressesRequestDto Map(GetUserAddressesRequest model, int userId);
}