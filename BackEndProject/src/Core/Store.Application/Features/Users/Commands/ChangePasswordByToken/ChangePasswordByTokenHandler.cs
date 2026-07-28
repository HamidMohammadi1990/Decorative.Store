using Store.Common.Utilities;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Infrastructure;
using Store.Common.Models;
using Store.Common.Security;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public record ChangePasswordByTokenHandler
    (ISmsService smsService, IEmailService emailService, IUserRepository userRepository, IAccountingService accountingService, IUserSessionService userSessionService, ICurrentUserContext currentUser, IPasswordHasher passwordHasher)
    : IRequestHandler<ChangePasswordByTokenRequest, OperationResult<ChangePasswordByTokenResponse>>
{
    public async Task<OperationResult<ChangePasswordByTokenResponse>> Handle(ChangePasswordByTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserNameAsync(request.UserName, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        var confirmResult =
            request.OptionType == ForgetPasswordOptionType.Message
            ? await smsService.VerifyTokenAsync(request.Token, user.PhoneNumber!)
            : await emailService.VerifyTokenAsync(request.Token, user.Email!);

        if (!confirmResult)
            return ErrorModel.Create("VerificationCodeIsNotCorrect");

        if (request.OptionType == ForgetPasswordOptionType.Message)
            user.ConfirmPhoneNumber();

        if (request.OptionType == ForgetPasswordOptionType.Email)
            user.ConfirmEmail();

        user.UpdatePassword(passwordHasher.HashPassword(request.Password));
        await userSessionService.RevokeAllSessionsAsync(user.Id, UserSessionRevokeReason.PasswordChanged, cancellationToken: cancellationToken);

        var tokenResponse = await accountingService.IssueTokenPairAsync(
            user,
            currentUser.GetSessionContext(),
            cancellationToken: cancellationToken);

        if (!tokenResponse.IsSuccess)
            return OperationResult<ChangePasswordByTokenResponse>.Fail();

        return new ChangePasswordByTokenResponse
        {
            ExpiresIn = tokenResponse.Result!.ExpiresIn,
            TokenType = tokenResponse.Result.TokenType,
            AccessToken = tokenResponse.Result.AccessToken,
            RefreshToken = tokenResponse.Result.RefreshToken,
            SessionId = tokenResponse.Result.SessionId
        };
    }
}