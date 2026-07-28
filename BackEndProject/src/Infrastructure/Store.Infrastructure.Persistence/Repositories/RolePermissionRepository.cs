using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RolePermissions;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository
    (EditionDbContext context)
    : Repository<RolePermission>(context), IRolePermissionRepository
{
    public async Task<GetRolePermissionDto?> GetInfoAsync(int id)
    {
        return await
            (from rolePermission in Context.RolePermission.AsNoTracking()
             join role in Context.Role on rolePermission.RoleId equals role.Id
             join permission in Context.Permission on rolePermission.PermissionId equals permission.Id
             where rolePermission.Id == id
             select new GetRolePermissionDto
             {
                 Id = rolePermission.Id,
                 RoleId = rolePermission.RoleId,
                 RoleTitle = role.Title,
                 PermissionId = rolePermission.PermissionId,
                 PermissionTitle = permission.Title
             })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResult<GetAllRolePermissionDto>> GetAllAsync(GetAllRolePermissionRequestDto request)
    {
        var rolePermissionSource = Context.RolePermission
            .ApplyContentPolicyFilter(request.ContentFilter);

        var rolePermissions =
            from rolePermission in rolePermissionSource
            join role in Context.Role on rolePermission.RoleId equals role.Id
            join permission in Context.Permission on rolePermission.PermissionId equals permission.Id
            select new { rolePermission, role, permission };

        rolePermissions = rolePermissions.ApplyQueryFilters(request);

        var result = await
            rolePermissions
            .Select(x => new GetAllRolePermissionDto
            {
                Id = x.rolePermission.Id,
                RoleId = x.rolePermission.RoleId,
                RoleTitle = x.role.Title,
                PermissionId = x.rolePermission.PermissionId,
                PermissionTitle = x.permission.Title
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}