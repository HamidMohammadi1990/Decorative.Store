using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Common.Security;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class UpdateUserHandler
    (IUnitOfWork uow, IUserRepository userRepository, IPasswordHasher passwordHasher)
    : IRequestHandler<UpdateUserRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindAsync(request.Id, cancellationToken);
        if (user is null)
            return ErrorModel.Create("InvalidId");

        var isExistsUser = await userRepository.AnyAsync(
            x => x.Id != request.Id &&
                 (x.UserName == request.UserName ||
                  x.UserName == request.PhoneNumber ||
                  x.PhoneNumber == request.UserName ||
                  x.PhoneNumber == request.PhoneNumber ||
                  x.Email == request.Email ||
                  x.UserName == request.Email),
            cancellationToken);
        if (isExistsUser)
            return ErrorModel.Create("AnAccountWithThisInfoHasAleardyBeenRegistered");

        user.UpdateByAdmin(
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Gender,
            request.IsActive,
            request.LoginPermission);

        user.SetProfileImageFileName(request.ProfileImageFileName);

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.UpdatePassword(passwordHasher.HashPassword(request.Password));

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
