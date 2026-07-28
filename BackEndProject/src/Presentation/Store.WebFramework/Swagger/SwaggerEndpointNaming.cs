using Microsoft.AspNetCore.Mvc.Controllers;

namespace Store.WebFramework.Swagger;

internal static class SwaggerEndpointNaming
{
    public static string FromAction(ControllerActionDescriptor descriptor)
    {
        var route = descriptor.AttributeRouteInfo?.Template?
            .Trim('/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault();

        return Humanize(string.IsNullOrWhiteSpace(route) ? descriptor.ActionName : route);
    }

    public static string Humanize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        if (name.Contains('-', StringComparison.Ordinal))
        {
            return string.Join(' ',
                name.Split('-', StringSplitOptions.RemoveEmptyEntries)
                    .Select(static part => char.ToUpperInvariant(part[0]) + part[1..]));
        }

        return string.Concat(name.Select((ch, index) =>
            index > 0 && char.IsUpper(ch) && !char.IsUpper(name[index - 1])
                ? $" {ch}"
                : ch.ToString()));
    }
}
