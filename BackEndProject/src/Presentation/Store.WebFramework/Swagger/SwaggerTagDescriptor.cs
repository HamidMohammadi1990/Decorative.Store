using Microsoft.AspNetCore.Mvc.Controllers;
using Store.Common.Enums;
using Store.Common.Extensions;

namespace Store.WebFramework.Swagger;

internal sealed record SwaggerTagDescriptor(
    string TagName,
    string Title,
    ApiControllerCategory Category,
    string Audience)
{
    public string CategoryDisplayName => Category.GetEnglishDisplayName();

    public static SwaggerTagDescriptor Resolve(ControllerActionDescriptor descriptor)
    {
        var controllerType = descriptor.ControllerTypeInfo.AsType();
        var audience = SwaggerControllerAudience.Resolve(descriptor);
        var title = HumanizeControllerName(descriptor.ControllerName);
        var category = ResolveCategory(controllerType, descriptor.ControllerName);

        return new SwaggerTagDescriptor(title, title, category, audience);
    }

    private static ApiControllerCategory ResolveCategory(Type controllerType, string controllerName)
    {
        var attribute = controllerType
            .GetCustomAttributes(inherit: true)
            .FirstOrDefault(x => x.GetType().FullName == "Edition.Api.Attributes.ApiControllerCategoryAttribute");

        if (attribute is not null)
        {
            var categoryValue = attribute.GetType().GetProperty("Category")?.GetValue(attribute);
            if (categoryValue is ApiControllerCategory category)
                return category;
        }

        throw new InvalidOperationException(
            $"Controller '{controllerName}' is missing ApiControllerCategoryAttribute.");
    }

    private static string HumanizeControllerName(string name)
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
