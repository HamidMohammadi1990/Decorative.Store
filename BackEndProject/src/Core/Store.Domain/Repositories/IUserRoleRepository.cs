using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.UserRoles;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserRoleRepository
{
    void Add(UserRole userRole);
    void Remove(UserRole userRole);
    ValueTask<UserRole?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<GetUserRoleDto?> GetInfoAsync(int id);
    Task<PagedResult<GetAllUserRoleDto>> GetAllAsync(GetAllUserRoleRequestDto request);
    Task<bool> AnyAsync(Expression<Func<UserRole, bool>> expression, CancellationToken cancellationToken = default);
}