using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Permissions.Commands;

public class CreatePermissionHandler
    (IUnitOfWork uow, IPermissionRepository permissionRepository)
    : IRequestHandler<CreatePermissionRequest, OperationResult<CreatePermissionResponse>>
{
    public async Task<OperationResult<CreatePermissionResponse>> Handle(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var permission = Permission.Create(
            request.Id,
            request.Url,
            request.Title,
            request.NameSpace,
            request.Priority,
            request.LevelTypeId,
            request.ParentId);

        permissionRepository.Add(permission);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePermissionResponse>();

        return new CreatePermissionResponse { Id = permission.Id };
    }
}
