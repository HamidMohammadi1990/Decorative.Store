using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Common.Security;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public class ChangePasswordByOldPasswordHandler
    (
        IUnitOfWork uow,
        ICurrentUserContext currentUser,
        IUserRepository userRepository,
        IUserSessionService userSessionService,
        IAccountingService accountingService,
        IPasswordHasher passwordHasher)
    : IRequestHandler<ChangePasswordByOldPasswordRequest, OperationResult<SignInUserResponse>>
{
    public async Task<OperationResult<SignInUserResponse>> Handle(ChangePasswordByOldPasswordRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var user = await userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        if (!passwordHasher.VerifyPassword(request.OldPassword, user.PasswordHash))
            return ErrorModel.Create("MismatchWithPreviousPassword");

        user.UpdatePassword(passwordHasher.HashPassword(request.NewPassword));

        var currentSessionId = currentUser.SessionId;
        await userSessionService.RevokeAllSessionsAsync(
            userId,
            UserSessionRevokeReason.PasswordChanged,
            currentSessionId,
            cancellationToken);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<SignInUserResponse>();

        var tokenResponse = await accountingService.IssueTokenPairAsync(
            user,
            currentUser.GetSessionContext(),
            currentSessionId,
            cancellationToken);

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