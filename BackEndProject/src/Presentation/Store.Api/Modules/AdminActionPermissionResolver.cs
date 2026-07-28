using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Store.WebFramework.Api;
using Store.Api.Attributes;
using Store.Common.Extensions;
using Store.Domain.Enums;

namespace Store.Api.Modules;

public static class AdminActionPermissionResolver
{
    public static PermissionType Resolve(MethodInfo method, AdminControllerPermissionMetadata metadata)
    {
        var actionInfo = method.GetCustomAttribute<ActionInfoAttribute>();
        if (actionInfo is not null)
            return actionInfo.PermissionType;

        return metadata.PageType;
    }

    public static string GetActionDisplayName(MethodInfo method, PermissionType permissionType)
    {
        var actionInfo = method.GetCustomAttribute<ActionInfoAttribute>();
        if (actionInfo is not null)
            return actionInfo.PermissionType.ToDisplay();

        return permissionType.ToDisplay();
    }

    public static string? GetRouteTemplate(MethodInfo method)
        => method.GetCustomAttributes(inherit: true)
            .OfType<HttpMethodAttribute>()
            .Select(x => x.Template?.Trim('/'))
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

    public static string BuildActionUrl(string controllerUrl, MethodInfo method)
    {
        var template = GetRouteTemplate(method);
        return string.IsNullOrWhiteSpace(template)
            ? $"{controllerUrl}/{method.Name.ToLowerInvariant()}"
            : $"{controllerUrl}/{template.ToLowerInvariant()}";
    }

    public static bool IsHttpEndpoint(MethodInfo method)
        => method.GetCustomAttributes(inherit: true)
            .Any(attribute => attribute is HttpGetAttribute
                or HttpPostAttribute
                or HttpPutAttribute
                or HttpDeleteAttribute
                or HttpPatchAttribute)
            && method.GetCustomAttribute<NonActionAttribute>() is null;

    public static bool RequiresAuthorization(Type controllerType, MethodInfo method)
    {
        if (method.GetCustomAttribute<AllowAnonymousAttribute>() is not null)
            return false;

        return typeof(BaseApiAdminController).IsAssignableFrom(controllerType);
    }

    public static bool HasActionPermission(MethodInfo method)
        => method.GetCustomAttribute<ActionInfoAttribute>() is not null;
}
