using Microsoft.AspNetCore.Mvc.Controllers;
using Store.WebFramework.Api;

namespace Store.WebFramework.Swagger;

internal static class SwaggerControllerAudience
{
    public static string Resolve(ControllerActionDescriptor descriptor)
    {
        if (string.Equals(descriptor.ControllerName, "account", StringComparison.OrdinalIgnoreCase))
            return "Auth";

        return IsAdmin(descriptor) ? "Admin" : "Public";
    }

    public static bool IsAdmin(ControllerActionDescriptor descriptor)
        => typeof(BaseApiAdminController).IsAssignableFrom(descriptor.ControllerTypeInfo.AsType());
}
