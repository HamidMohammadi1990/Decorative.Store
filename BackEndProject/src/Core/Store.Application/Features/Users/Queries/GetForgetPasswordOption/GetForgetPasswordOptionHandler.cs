using Edition.Application.Contracts;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries;

public class GetForgetPasswordOptionHandler
    (IUserRepository userRepository, IAccountingService accountingService, IUserMapperService mapper)
    : IRequestHandler<GetForgetPasswordOptionRequest, OperationResult<GetForgetPasswordOptionResponse>>
{
    public async Task<OperationResult<GetForgetPasswordOptionResponse>> Handle(GetForgetPasswordOptionRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        if (!user.LoginPermission)
            return ErrorModel.Create("AccessDenied");

        if (!user.IsActive)
            return ErrorModel.Create("UserIsInActive");

        if (!user.PhoneNumberConfirmed && !user.EmailConfirmed)
            return ErrorModel.Create("MobileOrEmailIsNotVerified");

        var optionItems = accountingService.GetForgetPasswordOptionsByUser(user);

        if (optionItems.Count == 0)
            return ErrorModel.Create("ContactSupportToRecoverYourPassword");

        var options = mapper.Map(request.UserName, optionItems);
        return options;
    }
}