using Asp.Versioning;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Store.Common.Extensions;
using Store.Domain.Dtos.Others;

namespace Store.Api.Modules;

/// <summary>
/// Discovers admin API controllers and their secured endpoints for permission seeding.
/// </summary>
public static class PermissionModule
{
    public static List<DynamicPermission> GetPermissions()
    {
        var assembly = typeof(PermissionModule).Assembly;
        var controllers = AdminPermissionDiscovery
            .GetAdminControllers(assembly)
            .Select(BuildControllerPermission)
            .Where(controller => controller.Actions.Count > 0)
            .ToList();

        var result = new List<DynamicPermission>();
        foreach (var group in controllers.GroupBy(controller => controller.GroupType))
        {
            var permission = new DynamicPermission { Name = group.Key.ToDisplay() };
            foreach (var controller in group.OrderBy(x => x.Name))
                permission.Controllers.Add(controller);

            result.Add(permission);
        }

        return result;
    }

    private static PermissionController BuildControllerPermission(Type controllerType)
    {
        AdminPermissionDiscovery.TryGetMetadata(controllerType, out var metadata);

        var controllerUrl = BuildControllerUrl(controllerType);
        var actions = controllerType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(method => AdminActionPermissionResolver.IsHttpEndpoint(method)
                             && AdminActionPermissionResolver.RequiresAuthorization(controllerType, method)
                             && AdminActionPermissionResolver.HasActionPermission(method))
            .Select(method =>
            {
                var permissionType = AdminActionPermissionResolver.Resolve(method, metadata);
                return new
                {
                    ActionUrl = AdminActionPermissionResolver.BuildActionUrl(controllerUrl, method),
                    ActionName = AdminActionPermissionResolver.GetActionDisplayName(method, permissionType),
                    ActionFullName = $"{controllerType.FullName}.{method.Name}",
                    PermissionType = permissionType
                };
            })
            .ToList();

        var permissionController = new PermissionController
        {
            Url = controllerUrl,
            Name = AdminPermissionDiscovery.GetDisplayName(metadata),
            FullName = controllerType.FullName ?? controllerType.Name,
            Type = metadata.PageType,
            GroupType = metadata.GroupType,
            Actions = []
        };

        foreach (var action in actions.GroupBy(x => new { x.ActionName, x.PermissionType }))
        {
            var first = action.First();
            permissionController.Actions.Add(new PermissionAction
            {
                Url = first.ActionUrl,
                Name = action.Key.ActionName,
                Type = action.Key.PermissionType,
                FullNames = action.Select(x => x.ActionFullName).Distinct().ToList()
            });
        }

        return permissionController;
    }

    private static string BuildControllerUrl(Type controllerType)
    {
        var version = controllerType
            .GetCustomAttributes<ApiVersionAttribute>(inherit: false)
            .SelectMany(x => x.Versions)
            .FirstOrDefault()
            ?.ToString() ?? "1";

        var controllerName = controllerType
            .GetCustomAttribute<ControllerNameAttribute>()?.Name
            ?? controllerType.Name.Replace("Controller", string.Empty, StringComparison.Ordinal);

        return $"/api/v{version}/admin/{controllerName.ToLowerInvariant()}";
    }
}