using System.Linq.Expressions;
using Store.Domain.Dtos.Roles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IRoleRepository
{
    void Add(Role role);
    void Remove(Role role);
    ValueTask<Role?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Role?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Role>> GetAllAsync(GetAllRoleRequestDto request);
    Task<bool> AnyAsync(Expression<Func<Role, bool>> expression, CancellationToken cancellationToken = default);
}