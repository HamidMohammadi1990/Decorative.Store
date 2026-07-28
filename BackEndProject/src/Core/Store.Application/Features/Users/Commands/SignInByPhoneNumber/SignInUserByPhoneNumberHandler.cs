using Edition.Application.Contracts;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class SignInUserByPhoneNumberHandler
    (ISmsService smsService, IUserRepository userRepository, IAccountingService accountingService, ICurrentUserContext currentUser)
    : IRequestHandler<SignInUserByPhoneNumberRequest, OperationResult<SignInUserResponse>>
{
    public async Task<OperationResult<SignInUserResponse>> Handle(SignInUserByPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        var verifyResult = await smsService.VerifyTokenAsync(request.Token, user.PhoneNumber!);
        if (!verifyResult)
            return ErrorModel.Create("VerificationCodeIsNotValid");

        var tokenResponse = await accountingService.IssueTokenPairAsync(
            user,
            currentUser.GetSessionContext(),
            cancellationToken: cancellationToken);

        if (!tokenResponse.IsSuccess)
            return OperationResult<SignInUserResponse>.Fail();

        return new SignInUserResponse
        {
            ExpiresIn = tokenResponse.Result!.ExpiresIn,
            TokenType = tokenResponse.Result.TokenType,
            AccessToken = tokenResponse.Result.AccessToken,
            RefreshToken = tokenResponse.Result.RefreshToken,
            SessionId = tokenResponse.Result.SessionId
        };
    }
}