using Store.Domain.Enums;
using Store.Domain.Entities;
using Store.Common.Utilities;
using Store.Domain.Dtos.Others;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Contracts;

namespace Store.Infrastructure.Persistence.SeedData;

public class SeedService(EditionDbContext context) : ISeedService
{
    public async Task SeedDataAsync(List<DynamicPermission> dynamicPermissions)
    {
        if (dynamicPermissions.Count > 0)
            await SeedPermissionsAsync(dynamicPermissions);

        //if (permissionIds.Count != 0)
        //    await SeedRolePermissionsAsync(permissionIds, adminRoleId);
    }

    private async Task SeedContentPoliciesAsync()
    {
        var adminRoles = await context.Role
            .Where(x => x.Title == "مدیر" || x.Title.Contains("مدیر"))
            .ToListAsync();

        foreach (var adminRole in adminRoles)
            adminRole.SetBypassContentPolicy(true);

        var bloggerRole = await context.Role.FirstOrDefaultAsync(x => x.Title == "بلاگر");
        if (bloggerRole is null)
        {
            bloggerRole = Role.Create("بلاگر");
            bloggerRole.SetRequireContentPolicy(true);
            context.Role.Add(bloggerRole);
        }
        else
        {
            bloggerRole.SetRequireContentPolicy(true);
        }

        await context.SaveChangesAsync();
    }

    private async Task<int> SeedRolesAsync()
    {
        var roles = new List<Role>
        {
            Role.Create("مدیر"),
            Role.Create("فروشنده")
        };
        var existsRoles = await context.Role.AsNoTracking().ToListAsync();
        roles = roles.Where(role => !existsRoles.Any(x => x.Title == role.Title)).ToList();

        if (roles.Count != 0)
        {
            context.Role.AddRange(roles);
            await context.SaveChangesAsync();
        }

        var adminRoleId =
            roles.Count != 0
            ? roles.First().Id
            : existsRoles.Single(x => x.Title == "مدیر" || x.Title.Contains("مدیر")).Id;

        return adminRoleId;
    }

    private async Task<List<int>> SeedPermissionsAsync(List<DynamicPermission> dynamicPermissions)
    {
        var priority = 1;
        foreach (var dynamicPermission in dynamicPermissions)
        {
            var existsTabPermission = await context
                             .Permission
                             .FirstOrDefaultAsync(x => x.Title == dynamicPermission.Name && x.ParentId == PermissionType.Product);

            var tabPermission = existsTabPermission ??
                                Permission.Create(dynamicPermission.Controllers[0].GroupType, "", dynamicPermission.Name, "",
                                                  priority, PermissionLevelType.Tab, PermissionType.Product);
            tabPermission.SetParents([]);

            var versionOfControllers = dynamicPermission.Controllers.GroupBy(x => GetControllerName(x.FullName)).ToList();
            foreach (var controller in versionOfControllers)
            {
                var actions = controller.ToList().SelectMany(x => x.Actions).DistinctBy(x => new { x.Name, x.Type }).ToList();
                var firstController = controller.First();
                var existsPagePermission =
                        await context.Permission
                       .SingleOrDefaultAsync(x => x.Title == firstController.Name && x.NameSpace == firstController.FullName);

                var pagePermission = existsPagePermission ??
                                     Permission.Create(firstController.Type, firstController.Url, firstController.Name,
                                     firstController.FullName, ++priority, PermissionLevelType.Page, tabPermission.Id);
                pagePermission.SetParents([]);

                foreach (var action in actions)
                {
                    if (action.Type == pagePermission.Id)
                        continue;

                    var isExistsAction = await context.Permission.AnyAsync(x => x.Title == action.Name && x.NameSpace == action.FullNames.FirstOrDefault());
                    if (isExistsAction)
                        continue;

                    var pagePermissionAction =
                            Permission.Create(action.Type, action.Url, action.Name, action.FullNames.FirstOrDefault() ?? "",
                                              ++priority, PermissionLevelType.Action, pagePermission.Id);

                    if (!pagePermission.Children.Any(x => x.Title == pagePermissionAction.Title && x.NameSpace == pagePermissionAction.NameSpace))
                        pagePermission.Children.Add(pagePermissionAction);
                }

                if (!tabPermission.Children.Any(x => x.Title == pagePermission.Title && x.NameSpace == pagePermission.NameSpace))
                    tabPermission.Children.Add(pagePermission);
            }
            if (existsTabPermission is null)
                context.Permission.Add(tabPermission);
            else
                context.Permission.Update(tabPermission);

            priority++;
        }
        await context.SaveChangesAsync();

        var permissionIds =
                context.ChangeTracker
               .Entries<Permission>()
               .SelectMany(x => x.Properties)
               .Where(x => x.Metadata.Name == "Id")
               .Select(x => Convert.ToInt32(x.CurrentValue))
               .ToList();

        return permissionIds;

        static string GetControllerName(string fullName)
           => fullName.Split(".").Last();
    }

    private async Task SeedUsersAsync(int adminRoleId, int meshkinDashtCityId)
    {
        var adminUserRole = UserRole.Create(adminRoleId);

        List<User> users =
            [
                User.Create("hamidmohammadnian@gmail.com", meshkinDashtCityId, GenderType.Male, "09307653782", "Hamid", "Mohammadnian", "09307653782",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ]),

                User.Create("hosseinojaq@gmail.com", meshkinDashtCityId, GenderType.Male, "09383109379", "Hossein", "Ojaq", "09383109379",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ])
            ];

        foreach (var user in users)
        {
            if (!await context.User.AnyAsync(x => x.UserName == user.UserName))
                context.User.Add(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task SeedRolePermissionsAsync(List<int> permissionIds, int adminRoleId)
    {
        var rolePermissionIds =
             context.RolePermission
            .Where(x => permissionIds.Contains(x.Id))
            .AsNoTracking()
            .Select(x => x.Id)
            .ToList();

        permissionIds =
            permissionIds
            .Where(x => !rolePermissionIds
            .Contains(x))
            .ToList();

        var rolePermissionsForAdmin = permissionIds.Select(x => RolePermission.Create(adminRoleId, (PermissionType)x!)).ToList();
        context.RolePermission.AddRange(rolePermissionsForAdmin);
        await context.SaveChangesAsync();
    }
}