using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Permissions;
using Store.Domain.Enums;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PermissionRepository
    (EditionDbContext context)
    : Repository<Permission, PermissionType>(context), IPermissionRepository
{
    public async Task<PagedResult<Permission>> GetAllAsync(GetAllPermissionRequestDto request)
    {
        var permissions = Context.Permission
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result = await
            permissions
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<bool> HasPermissionAsync(int userId, PermissionType permissionType)
    {
        return await (from UserRole in Context.UserRole
                      join RolePermission in Context.RolePermission
                      on UserRole.RoleId equals RolePermission.RoleId
                      where UserRole.UserId == userId
                      && RolePermission.PermissionId == permissionType
                      select RolePermission.PermissionId)
                   .AnyAsync();
    }
}