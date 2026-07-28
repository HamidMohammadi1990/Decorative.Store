using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Store.Common.Enums;
using Store.Common.Resources;

namespace Store.Common.Extensions;

public static class ApiControllerCategoryExtensions
{
    private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");

    public static string GetEnglishDisplayName(this ApiControllerCategory category)
        => category.GetDisplayName(EnglishCulture);

    public static string GetDisplayName(this ApiControllerCategory category, CultureInfo? culture = null)
    {
        var member = typeof(ApiControllerCategory).GetMember(category.ToString()).FirstOrDefault();
        var display = member?.GetCustomAttribute<DisplayAttribute>();
        if (display?.ResourceType is not null && display.Name is not null)
        {
            var value = ControllerCategoryResources.ResourceManager.GetString(display.Name, culture);
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return category.ToString();
    }
}
