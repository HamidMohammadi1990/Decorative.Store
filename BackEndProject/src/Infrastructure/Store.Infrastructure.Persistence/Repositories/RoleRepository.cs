using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Roles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class RoleRepository
    (EditionDbContext context)
    : Repository<Role>(context), IRoleRepository
{
    public async Task<PagedResult<Role>> GetAllAsync(GetAllRoleRequestDto request)
    {
        var roles = await Context.Role
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
        
        return roles;
    }
}