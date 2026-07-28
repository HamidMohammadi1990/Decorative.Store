using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Dtos.UserRoles;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserRoleRepository
    (EditionDbContext context)
    : Repository<UserRole>(context), IUserRoleRepository
{
    public async Task<GetUserRoleDto?> GetInfoAsync(int id)
    {
        return await
            (from userRole in Context.UserRole.AsNoTracking()
             join user in Context.User on userRole.UserId equals user.Id
             join role in Context.Role on userRole.RoleId equals role.Id
             where userRole.Id == id
             select new GetUserRoleDto
             {
                 Id = userRole.Id,
                 UserId = userRole.UserId,
                 UserName = user.UserName,
                 RoleId = userRole.RoleId,
                 RoleTitle = role.Title
             })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<GetAllUserRoleDto>> GetAllAsync(GetAllUserRoleRequestDto request)
    {
        var userRoleSource = Context.UserRole
            .ApplyContentPolicyFilter(request.ContentFilter);

        var userRoles =
            from userRole in userRoleSource
            join user in Context.User on userRole.UserId equals user.Id
            join role in Context.Role on userRole.RoleId equals role.Id
            select new { userRole, user, role };

        userRoles = userRoles.ApplyQueryFilters(request);

        var result = await
            userRoles
            .Select(x => new GetAllUserRoleDto
            {
                Id = x.userRole.Id,
                UserId = x.userRole.UserId,
                UserName = x.user.UserName,
                RoleId = x.userRole.RoleId,
                RoleTitle = x.role.Title
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}