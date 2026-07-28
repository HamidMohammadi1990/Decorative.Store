using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RolePermissions.Commands;

public class DeleteRolePermissionHandler
    (IUnitOfWork uow, IRolePermissionRepository rolePermissionRepository)
    : IRequestHandler<DeleteRolePermissionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteRolePermissionRequest request, CancellationToken cancellationToken)
    {
        var rolePermission = await rolePermissionRepository.FindAsync(request.Id);
        if (rolePermission is null)
            return ErrorModel.Create("InvalidId");

        rolePermissionRepository.Remove(rolePermission);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
