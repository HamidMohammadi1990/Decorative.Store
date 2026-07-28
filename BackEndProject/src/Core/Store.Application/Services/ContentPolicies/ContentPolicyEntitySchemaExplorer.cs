using System.Collections;
using System.Reflection;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.Enums;
using Store.Domain.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyEntitySchemaExplorer(IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyEntitySchemaExplorer
{
    public IReadOnlyList<ContentPolicyEntityTypeOptionDto> GetEntityTypes()
        => [.. entityTypeRegistry.GetRegisteredEntityTypeNames()
            .Select(name => new ContentPolicyEntityTypeOptionDto(name))];

    public IReadOnlyList<ContentPolicySchemaPropertyDto> GetProperties(string entityType, string? parentPath = null)
    {
        if (!entityTypeRegistry.IsRegistered(entityType))
            return [];

        var entityPrefix = entityTypeRegistry.GetEntityPrefix(entityType);
        var (currentType, currentPath) = ResolvePathType(entityType, entityPrefix, parentPath);
        if (currentType is null)
            return [];

        return currentType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(property => MapProperty(entityPrefix, currentPath, property))
            .ToList();
    }

    public IReadOnlyList<ContentPolicyOperator> GetAllowedOperators(string entityType, string fieldPath)
    {
        if (!TryResolveFieldPath(entityType, fieldPath, out var leafType, out var kind))
            return [];

        return ContentPolicyOperatorCompatibility.GetOperators(kind, leafType);
    }

    public IReadOnlyList<ContentPolicyContextPathDto> GetContextPaths()
        => typeof(ContentPolicyContext)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(x => x.Name, StringComparer.Ordinal)
            .Select(property => new ContentPolicyContextPathDto(
                property.Name,
                GetFriendlyTypeName(property.PropertyType),
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string)))
            .ToList();

    private ContentPolicySchemaPropertyDto MapProperty(string entityPrefix, string parentPath, PropertyInfo property)
    {
        var propertyName = property.Name;
        var path = string.IsNullOrEmpty(parentPath)
            ? $"{entityPrefix}.{propertyName}"
            : $"{parentPath}.{propertyName}";

        var propertyType = property.PropertyType;
        var kind = GetPropertyKind(propertyType);
        var isExpandable = kind is ContentPolicyPropertyKind.Navigation or ContentPolicyPropertyKind.Collection;
        var leafType = kind == ContentPolicyPropertyKind.Collection
            ? GetCollectionElementType(propertyType) ?? propertyType
            : propertyType;

        return new ContentPolicySchemaPropertyDto(
            propertyName,
            path,
            kind,
            GetFriendlyTypeName(propertyType),
            IsNullableType(propertyType),
            isExpandable,
            ContentPolicyOperatorCompatibility.GetOperators(kind, leafType));
    }

    private (Type? CurrentType, string Path) ResolvePathType(string entityType, string entityPrefix, string? parentPath)
    {
        if (string.IsNullOrWhiteSpace(parentPath))
            return (entityTypeRegistry.GetClrType(entityType), entityPrefix);

        if (!TryResolveFieldPath(entityType, parentPath, out var leafType, out var kind))
            return (null, parentPath);

        if (kind == ContentPolicyPropertyKind.Collection)
        {
            var elementType = GetCollectionElementType(leafType);
            return elementType is null ? (null, parentPath) : (elementType, parentPath);
        }

        if (kind == ContentPolicyPropertyKind.Navigation)
            return (leafType, parentPath);

        return (null, parentPath);
    }

    private bool TryResolveFieldPath(
        string entityType,
        string fieldPath,
        out Type leafType,
        out ContentPolicyPropertyKind kind)
    {
        leafType = entityTypeRegistry.GetClrType(entityType);
        kind = ContentPolicyPropertyKind.Scalar;

        var segments = fieldPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments.Length < 2)
            return false;

        var expectedPrefix = entityTypeRegistry.GetEntityPrefix(entityType);
        if (!string.Equals(segments[0], expectedPrefix, StringComparison.Ordinal))
            return false;

        var currentType = leafType;
        for (var i = 1; i < segments.Length; i++)
        {
            var segment = segments[i];
            var property = ContentPolicyPropertyReflectionCache.GetProperty(currentType, segment);
            if (property is null)
                return false;

            var propertyType = property.PropertyType;
            var remaining = segments[(i + 1)..];

            if (remaining.Length > 0 && IsCollection(propertyType))
            {
                var elementType = GetCollectionElementType(propertyType);
                if (elementType is null)
                    return false;

                currentType = elementType;
                if (!TryWalkSegments(elementType, remaining, out leafType))
                    return false;

                kind = GetPropertyKind(leafType);
                return true;
            }

            currentType = propertyType;
        }

        leafType = currentType;
        kind = GetPropertyKind(leafType);
        return true;
    }

    private static bool TryWalkSegments(Type startType, string[] segments, out Type leafType)
    {
        leafType = startType;
        foreach (var segment in segments)
        {
            var property = ContentPolicyPropertyReflectionCache.GetProperty(leafType, segment);
            if (property is null)
                return false;

            leafType = property.PropertyType;
        }

        return true;
    }

    private static ContentPolicyPropertyKind GetPropertyKind(Type type)
    {
        if (IsCollection(type))
            return ContentPolicyPropertyKind.Collection;

        var underlying = Nullable.GetUnderlyingType(type) ?? type;
        if (underlying.IsClass && underlying != typeof(string))
            return ContentPolicyPropertyKind.Navigation;

        return ContentPolicyPropertyKind.Scalar;
    }

    private static bool IsCollection(Type type)
        => type != typeof(string)
           && type != typeof(byte[])
           && typeof(IEnumerable).IsAssignableFrom(type);

    private static Type? GetCollectionElementType(Type collectionType)
    {
        if (collectionType.IsArray)
            return collectionType.GetElementType();

        if (collectionType.IsGenericType)
            return collectionType.GetGenericArguments()[0];

        return collectionType.GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }

    private static bool IsNullableType(Type type)
        => !type.IsValueType || Nullable.GetUnderlyingType(type) is not null;

    private static string GetFriendlyTypeName(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlying)
            return $"{GetFriendlyTypeName(underlying)}?";

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return $"IEnumerable<{GetFriendlyTypeName(type.GetGenericArguments()[0])}>";

        if (type.IsArray)
            return $"{GetFriendlyTypeName(type.GetElementType()!)}[]";

        return type.Name;
    }
}
