using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserAddresses.Queries;

public record GetUserAddressesHandler
	: IRequestHandler<GetUserAddressesRequest, OperationResult<PagedResult<GetUserAddressesResponse>>>
{
	private readonly IUserAddressMapperService mapper;
	private readonly ICurrentUserContext currentUser;
	private readonly IUserAddressRepository userAddressRepository;

	public GetUserAddressesHandler(
		ICurrentUserContext currentUser,
		IUserAddressRepository userAddressRepository,
		IUserAddressMapperService mapper)
	{
		this.mapper = mapper;
		this.currentUser = currentUser;
		this.userAddressRepository = userAddressRepository;
	}

	public async Task<OperationResult<PagedResult<GetUserAddressesResponse>>> Handle(GetUserAddressesRequest request, CancellationToken cancellationToken)
	{
		var userId = currentUser.UserId;
		var requestModel = mapper.Map(request, userId);
		var addresses = await userAddressRepository.GetUserAddressAsync(requestModel);
		var result = mapper.Map(addresses);
		return result;
	}
}
