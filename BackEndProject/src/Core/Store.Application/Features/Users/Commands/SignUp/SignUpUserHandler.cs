using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Extensions;
using Store.Common.Models;
using Store.Common.Security;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class SignUpUserHandler
    (
        IUserRepository userRepository,
        IAccountingService accountingService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ICurrentUserContext currentUser)
    : IRequestHandler<SignUpUserRequest, OperationResult<SignInUserResponse>>
{
    public async Task<OperationResult<SignInUserResponse>> Handle(
        SignUpUserRequest request,
        CancellationToken cancellationToken)
    {
        var userName = request.UserName.Trim();
        var isMobile = userName.IsMobile();

        if (await userRepository.FindByLoginAsync(userName, cancellationToken) is not null)
            return ErrorModel.Create("AnAccountWithThisInfoHasAleardyBeenRegistered");

        var passwordHash = passwordHasher.HashPassword(request.Password);
        var user = isMobile
            ? User.Create(userName, null, userName)
            : User.Create(userName, userName, null);

        user.CompleteStorefrontSignUp(
            request.FirstName.Trim(),
            request.LastName.Trim(),
            passwordHash,
            isMobile);

        userRepository.Add(user);

        var saveChangesResult = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<SignInUserResponse>();

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
