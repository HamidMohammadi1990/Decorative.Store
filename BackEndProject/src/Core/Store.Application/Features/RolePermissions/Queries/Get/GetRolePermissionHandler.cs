using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RolePermissions.Queries;

public class GetRolePermissionHandler
    (IRolePermissionRepository rolePermissionRepository, IRolePermissionMapperService mapper)
    : IRequestHandler<GetRolePermissionRequest, OperationResult<GetRolePermissionResponse?>>
{
    public async Task<OperationResult<GetRolePermissionResponse?>> Handle(GetRolePermissionRequest request, CancellationToken cancellationToken)
    {
        var rolePermission = await rolePermissionRepository.GetInfoAsync(request.Id);
        if (rolePermission is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(rolePermission);
    }
}