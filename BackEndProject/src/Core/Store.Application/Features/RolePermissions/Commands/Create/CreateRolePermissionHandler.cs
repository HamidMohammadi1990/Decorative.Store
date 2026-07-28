using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.RolePermissions.Commands;

public class CreateRolePermissionHandler
    (IUnitOfWork uow, IRolePermissionRepository rolePermissionRepository)
    : IRequestHandler<CreateRolePermissionRequest, OperationResult<CreateRolePermissionResponse>>
{
    public async Task<OperationResult<CreateRolePermissionResponse>> Handle(CreateRolePermissionRequest request, CancellationToken cancellationToken)
    {
        var rolePermission = RolePermission.Create(request.RoleId, request.PermissionId);
        rolePermissionRepository.Add(rolePermission);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateRolePermissionResponse>();

        return new CreateRolePermissionResponse { Id = rolePermission.Id };
    }
}
