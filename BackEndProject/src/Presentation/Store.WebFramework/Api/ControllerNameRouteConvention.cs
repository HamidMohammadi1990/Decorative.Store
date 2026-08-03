using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Store.WebFramework.Api;

/// <summary>
/// Resolves [controller] route tokens from <see cref="ControllerNameAttribute"/>.
/// </summary>
public sealed class ControllerNameRouteConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var controllerName = ResolveControllerName(controller);
            controller.ControllerName = controllerName;

            ReplaceControllerToken(controller.Selectors, controllerName);

            foreach (var action in controller.Actions)
                ReplaceControllerToken(action.Selectors, controllerName);
        }
    }

    private static string ResolveControllerName(ControllerModel controller)
    {
        var attribute = controller.Attributes.OfType<ControllerNameAttribute>().FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(attribute?.Name))
            return attribute.Name;

        var typeName = controller.ControllerType.Name;
        return typeName.EndsWith("Controller", StringComparison.Ordinal)
            ? typeName[..^"Controller".Length]
            : typeName;
    }

    private static void ReplaceControllerToken(IList<SelectorModel> selectors, string controllerName)
    {
        foreach (var selector in selectors)
        {
            var routeModel = selector.AttributeRouteModel;
            if (routeModel?.Template is not string template)
                continue;

            if (!template.Contains("[controller]", StringComparison.OrdinalIgnoreCase))
                continue;

            routeModel.Template = template.Replace("[controller]", controllerName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
