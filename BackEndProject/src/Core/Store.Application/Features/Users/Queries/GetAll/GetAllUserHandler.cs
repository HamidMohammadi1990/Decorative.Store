using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries.GetAll;

public class GetAllUserHandler
    (IUserRepository userRepository, IUserMapperService mapper)
    : IRequestHandler<GetAllUserRequest, OperationResult<PagedResult<GetAllUserResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllUserResponse>>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var users = await userRepository.GetAllAsync(requestModel);
        var result = mapper.Map(users);
        return result;
    }
}