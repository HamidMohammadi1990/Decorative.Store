using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Permissions.Commands;

public class DeletePermissionHandler
    (IUnitOfWork uow, IPermissionRepository permissionRepository, IRolePermissionRepository rolePermissionRepository)
    : IRequestHandler<DeletePermissionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePermissionRequest request, CancellationToken cancellationToken)
    {
        if (request.Id == PermissionType.Product)
            return ErrorModel.Create("CannotDeleteRootPermission");

        var permission = await permissionRepository.FindAsync(request.Id);
        if (permission is null)
            return ErrorModel.Create("InvalidId");

        if (await permissionRepository.AnyAsync(x => x.ParentId == request.Id))
            return ErrorModel.Create("HasChildren");

        if (await rolePermissionRepository.AnyAsync(x => x.PermissionId == request.Id))
            return ErrorModel.Create("InUse");

        permissionRepository.Remove(permission);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
