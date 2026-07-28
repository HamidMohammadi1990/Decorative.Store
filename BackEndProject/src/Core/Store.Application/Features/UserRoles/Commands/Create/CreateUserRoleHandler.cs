using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserRoles.Commands;

public class CreateUserRoleHandler
    (IUnitOfWork uow, IUserRoleRepository userRoleRepository, IContentPolicyCache contentPolicyCache)
    : IRequestHandler<CreateUserRoleRequest, OperationResult<CreateUserRoleResponse>>
{
    public async Task<OperationResult<CreateUserRoleResponse>> Handle(CreateUserRoleRequest request, CancellationToken cancellationToken)
    {
        var userRole = UserRole.Create(request.UserId, request.RoleId);
        userRoleRepository.Add(userRole);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateUserRoleResponse>();

        await contentPolicyCache.InvalidateUserAsync(request.UserId, cancellationToken);
        return new CreateUserRoleResponse { Id = userRole.Id };
    }
}
