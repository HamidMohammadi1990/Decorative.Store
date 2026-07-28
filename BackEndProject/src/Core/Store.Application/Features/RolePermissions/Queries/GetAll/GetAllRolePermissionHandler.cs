using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RolePermissions.Queries;

public class GetAllRolePermissionHandler
    (IRolePermissionRepository rolePermissionRepository, IRolePermissionMapperService mapper)
    : IRequestHandler<GetAllRolePermissionRequest, OperationResult<PagedResult<GetAllRolePermissionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllRolePermissionResponse>>> Handle(GetAllRolePermissionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var rolePermissions = await rolePermissionRepository.GetAllAsync(requestModel);
        return mapper.Map(rolePermissions);
    }
}
