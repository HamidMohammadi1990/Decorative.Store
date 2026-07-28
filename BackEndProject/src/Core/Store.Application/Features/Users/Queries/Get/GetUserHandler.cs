using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries;

public class GetUserHandler
    (IUserRepository userRepository, IUserMapperService mapper, ICurrentUserContext currentUser)
    : IRequestHandler<GetUserRequest, OperationResult<GetUserResponse?>>
{
    public async Task<OperationResult<GetUserResponse?>> Handle(GetUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUser.UserId;
        if (currentUserId <= 0 || request.Id != currentUserId)
            return ErrorModel.Create("AccessDenied");

        var user = await userRepository.GetAsNoTrackingAsync(request.Id);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        var result = mapper.Map(user);
        return result;
    }
}