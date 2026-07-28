using System.Collections;
using System.Reflection;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public static class ContentPolicyValueResolver
{
    public static bool IsValidContextPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return ResolveContextValue(path, ValidationContext) is not null;
    }

    public static object? ResolveScalar(
        ContentPolicyValueType valueType,
        string value,
        Type targetType,
        ContentPolicyContext context)
        => valueType switch
        {
            ContentPolicyValueType.Literal => ParseLiteral(value, targetType),
            ContentPolicyValueType.Context => ResolveContextScalar(value, targetType, context),
            _ => null
        };

    public static IReadOnlyList<int>? ResolveIntList(
        ContentPolicyValueType valueType,
        string value,
        ContentPolicyContext context)
    {
        if (valueType == ContentPolicyValueType.Literal)
            return ParseIntList(value);

        if (valueType == ContentPolicyValueType.Context)
            return ToIntList(ResolveContextValue(value, context));

        return null;
    }

    public static (object? Min, object? Max)? ResolveBetweenBounds(
        ContentPolicyValueType valueType,
        string value,
        Type targetType,
        ContentPolicyContext context)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (valueType == ContentPolicyValueType.Context && value.Contains('|'))
        {
            var parts = value.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
                return null;

            var min = ResolveScalar(ContentPolicyValueType.Context, parts[0], targetType, context);
            var max = ResolveScalar(ContentPolicyValueType.Context, parts[1], targetType, context);
            return min is null || max is null ? null : (min, max);
        }

        var bounds = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (bounds.Length != 2)
            return null;

        var minValue = ResolveScalar(valueType, bounds[0], targetType, context);
        var maxValue = ResolveScalar(valueType, bounds[1], targetType, context);
        return minValue is null || maxValue is null ? null : (minValue, maxValue);
    }

    public static object? ResolveContextCollection(string path, ContentPolicyContext context)
        => ResolveContextValue(path, context);

    private static readonly ContentPolicyContext ValidationContext = new(1, [1, 2], [1]);

    private static object? ResolveContextScalar(string path, Type targetType, ContentPolicyContext context)
    {
        var resolved = ResolveContextValue(path, context);
        if (resolved is null)
            return null;

        return ConvertToTargetType(resolved, targetType);
    }

    private static object? ResolveContextValue(string path, ContentPolicyContext context)
    {
        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        object? current = context;
        var currentType = typeof(ContentPolicyContext);

        foreach (var segment in segments)
        {
            if (current is null)
                return null;

            var property = ContentPolicyPropertyReflectionCache.GetProperty(currentType, segment);
            if (property is null)
                return null;

            current = property.GetValue(current);
            currentType = property.PropertyType;
        }

        return current;
    }

    private static IReadOnlyList<int>? ToIntList(object? value)
    {
        if (value is null)
            return null;

        if (value is IReadOnlyList<int> readOnlyList)
            return readOnlyList;

        if (value is IEnumerable<int> enumerable)
            return enumerable.ToList();

        if (value is int single)
            return [single];

        return null;
    }

    private static IReadOnlyList<int>? ParseIntList(string value)
        => value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => int.TryParse(x, out var id) ? id : (int?)null)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToList();

    private static object? ParseLiteral(string value, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlying == typeof(string))
            return value;

        if (underlying == typeof(bool) && bool.TryParse(value, out var boolValue))
            return boolValue;

        if (underlying == typeof(int) && int.TryParse(value, out var intValue))
            return intValue;

        if (underlying == typeof(long) && long.TryParse(value, out var longValue))
            return longValue;

        if (underlying == typeof(decimal) && decimal.TryParse(value, out var decimalValue))
            return decimalValue;

        if (underlying == typeof(double) && double.TryParse(value, out var doubleValue))
            return doubleValue;

        if (underlying == typeof(float) && float.TryParse(value, out var floatValue))
            return floatValue;

        if (underlying == typeof(Guid) && Guid.TryParse(value, out var guidValue))
            return guidValue;

        if (underlying == typeof(DateTime) && DateTime.TryParse(value, out var dateValue))
            return dateValue;

        if (underlying == typeof(DateTimeOffset) && DateTimeOffset.TryParse(value, out var dateOffsetValue))
            return dateOffsetValue;

        if (underlying.IsEnum && Enum.TryParse(underlying, value, ignoreCase: true, out var enumValue))
            return enumValue;

        return null;
    }

    private static object? ConvertToTargetType(object value, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlying.IsInstanceOfType(value))
            return value;

        if (value is int intValue && underlying.IsEnum)
            return Enum.ToObject(underlying, intValue);

        if (underlying.IsEnum && value is string enumString && Enum.TryParse(underlying, enumString, ignoreCase: true, out var enumValue))
            return enumValue;

        try
        {
            return Convert.ChangeType(value, underlying);
        }
        catch
        {
            return null;
        }
    }
}
