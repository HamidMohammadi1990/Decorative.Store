using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RolePermissions;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IRolePermissionRepository
{
    void Add(RolePermission rolePermission);
    void Remove(RolePermission rolePermission);
    ValueTask<RolePermission?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<GetRolePermissionDto?> GetInfoAsync(int id);
    Task<PagedResult<GetAllRolePermissionDto>> GetAllAsync(GetAllRolePermissionRequestDto request);
    Task<bool> AnyAsync(Expression<Func<RolePermission, bool>> expression, CancellationToken cancellationToken = default);
}