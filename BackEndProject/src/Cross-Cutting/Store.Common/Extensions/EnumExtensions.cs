using System.Reflection;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Store.Common.Enums;
using Store.Common.Utilities;
using Store.Common.Extensions;

namespace Store.Common.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        foreach (var value in Enum.GetValues(input.GetType()))
            if ((input as Enum).HasFlag(value as Enum))
                yield return (T)value;
    }

    public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
    {
        Assert.NotNull(value, nameof(value));

        var field = value.GetType().GetField(value.ToString());
        if (field is null)
            return value.ToString();

        var attribute = field.GetCustomAttribute<DisplayAttribute>();
        if (attribute is null)
            return value.ToString();

        var culture = CultureInfo.CurrentUICulture;
        var text = property switch
        {
            DisplayProperty.Name => ResolveDisplayName(attribute, culture),
            DisplayProperty.Description => ResolveDisplayText(attribute, attribute.Description, culture),
            DisplayProperty.GroupName => ResolveDisplayText(attribute, attribute.GroupName, culture),
            DisplayProperty.ShortName => ResolveDisplayText(attribute, attribute.ShortName, culture),
            DisplayProperty.Prompt => ResolveDisplayText(attribute, attribute.Prompt, culture),
            DisplayProperty.Order => attribute.GetOrder()?.ToString(culture),
            _ => ResolveDisplayName(attribute, culture)
        };

        return string.IsNullOrWhiteSpace(text) ? value.ToString() : text;
    }

    public static Dictionary<int, string> ToDictionary(this Enum value)
    {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(p => Convert.ToInt32(p), q => q.ToDisplay());
    }

    private static string? ResolveDisplayName(DisplayAttribute attribute, CultureInfo culture)
    {
        if (TryGetResourceString(attribute.ResourceType, attribute.Name, culture, out var localized))
            return localized;

        try
        {
            return attribute.GetName();
        }
        catch (InvalidOperationException)
        {
            return attribute.Name;
        }
    }

    private static string? ResolveDisplayText(
        DisplayAttribute attribute,
        string? resourceKey,
        CultureInfo culture)
    {
        if (TryGetResourceString(attribute.ResourceType, resourceKey, culture, out var localized))
            return localized;

        if (string.IsNullOrWhiteSpace(resourceKey))
            return resourceKey;

        try
        {
            return resourceKey switch
            {
                _ when resourceKey == attribute.Description => attribute.GetDescription(),
                _ when resourceKey == attribute.GroupName => attribute.GetGroupName(),
                _ when resourceKey == attribute.ShortName => attribute.GetShortName(),
                _ when resourceKey == attribute.Prompt => attribute.GetPrompt(),
                _ => resourceKey
            };
        }
        catch (InvalidOperationException)
        {
            return resourceKey;
        }
    }

    private static bool TryGetResourceString(
        Type? resourceType,
        string? key,
        CultureInfo culture,
        out string? value)
    {
        value = null;
        if (resourceType is null || string.IsNullOrWhiteSpace(key))
            return false;

        value = GetResourceString(resourceType, key, culture);
        return !string.IsNullOrWhiteSpace(value);
    }

    private static string? GetResourceString(Type resourceType, string key, CultureInfo culture)
    {
        var resourceManagerProperty = resourceType.GetProperty(
            "ResourceManager",
            BindingFlags.Public | BindingFlags.Static);

        if (resourceManagerProperty?.GetValue(null) is not ResourceManager resourceManager)
            return null;

        return resourceManager.GetString(key, culture);
    }
}