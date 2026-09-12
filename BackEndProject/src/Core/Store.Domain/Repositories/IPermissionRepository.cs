using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Permissions;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPermissionRepository
{
    void Add(Permission permission);
    void Remove(Permission permission);
    ValueTask<Permission?> FindAsync(PermissionType id, CancellationToken cancellationToken = default);
    Task<Permission?> GetAsNoTrackingAsync(PermissionType id, CancellationToken cancellationToken = default);
    Task<PagedResult<Permission>> GetAllAsync(GetAllPermissionRequestDto request);
    Task<bool> HasPermissionAsync(int UserId, PermissionType PermissionType);
    Task<List<string>> GetPermissionCodesByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Permission, bool>> expression, CancellationToken cancellationToken = default);
}