using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries;

public class GetCurrentUserHandler
    (IUserRepository userRepository, IUserMapperService mapper, ICurrentUserContext currentUser)
    : IRequestHandler<GetCurrentUserRequest, OperationResult<GetUserResponse?>>
{
    public async Task<OperationResult<GetUserResponse?>> Handle(
        GetCurrentUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = currentUser.UserId;
        if (userId <= 0)
            return ErrorModel.Create("AccessDenied");

        var user = await userRepository.GetAsNoTrackingAsync(userId);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        return mapper.Map(user);
    }
}
