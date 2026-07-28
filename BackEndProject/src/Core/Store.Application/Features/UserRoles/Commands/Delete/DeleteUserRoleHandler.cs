using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserRoles.Commands;

public class DeleteUserRoleHandler
    (IUnitOfWork uow, IUserRoleRepository userRoleRepository, IContentPolicyCache contentPolicyCache)
    : IRequestHandler<DeleteUserRoleRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteUserRoleRequest request, CancellationToken cancellationToken)
    {
        var userRole = await userRoleRepository.FindAsync(request.Id);
        if (userRole is null)
            return ErrorModel.Create("InvalidId");

        var userId = userRole.UserId;
        userRoleRepository.Remove(userRole);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        await contentPolicyCache.InvalidateUserAsync(userId, cancellationToken);
        return OperationResult.Success();
    }
}
