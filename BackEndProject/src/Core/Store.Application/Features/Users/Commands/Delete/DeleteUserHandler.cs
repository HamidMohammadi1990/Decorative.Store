using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public class DeleteUserHandler
    (IUnitOfWork uow, IUserRepository userRepository, ICurrentUserContext currentUser)
    : IRequestHandler<DeleteUserRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId > 0 && request.Id == currentUser.UserId)
            return ErrorModel.Create("AccessDenied");

        var user = await userRepository.FindAsync(request.Id, cancellationToken);
        if (user is null)
            return ErrorModel.Create("InvalidId");

        if (!user.IsActive)
            return OperationResult.Success();

        user.Deactivate();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
