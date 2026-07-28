using System.Collections;

namespace Edition.Application.Services.ContentPolicies;

internal static class ContentPolicyTypeReflection
{
    internal const int MaxPathDepth = 12;

    internal static bool IsCollection(Type type)
        => type != typeof(string)
           && type != typeof(byte[])
           && typeof(IEnumerable).IsAssignableFrom(type);

    internal static bool IsNavigation(Type type)
    {
        var underlying = Nullable.GetUnderlyingType(type) ?? type;
        return underlying.IsClass && underlying != typeof(string) && !IsCollection(underlying);
    }

    internal static Type? GetCollectionElementType(Type collectionType)
    {
        if (collectionType.IsArray)
            return collectionType.GetElementType();

        if (collectionType.IsGenericType)
            return collectionType.GetGenericArguments()[0];

        return collectionType.GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }
}
