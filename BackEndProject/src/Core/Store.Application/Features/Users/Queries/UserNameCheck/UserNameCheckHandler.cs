using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Common.Extensions;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries;

public class UserNameCheckHandler
    (IUserRepository userRepository, IUserMapperService mapper)
    : IRequestHandler<UserNameCheckRequest, OperationResult<UserNameCheckResponse>>
{
    public async Task<OperationResult<UserNameCheckResponse>> Handle(UserNameCheckRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
            return mapper.Map(false, request.UserName.IsMobile());

        if (!user.PhoneNumberConfirmed && !user.EmailConfirmed)
            return ErrorModel.Create("EmailOrMobileIsNotVerified");

        if (!user.LoginPermission)
            return ErrorModel.Create("AccessDenied");

        if (!user.IsActive)
            return ErrorModel.Create("UserIsInActive");

        return mapper.Map(true, request.UserName.IsMobile());
    }
}