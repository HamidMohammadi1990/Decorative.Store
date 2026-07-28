using System.Reflection;
using System.Collections;
using System.Collections.Concurrent;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyResourceRequestResolver
    (IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyResourceRequestResolver
{
    private static readonly ConcurrentDictionary<Type, bool> CollectionResponseCache = new();

    public bool TryResolve(object request, out string entityTypeName, out int resourceId)
    {
        entityTypeName = default!;
        resourceId = 0;

        if (request is IContentPolicyFilteredRequest)
            return false;

        if (request is IContentPolicyResourceRequest explicitRequest)
        {
            if (!entityTypeRegistry.IsRegistered(explicitRequest.EntityTypeName))
                return false;

            entityTypeName = explicitRequest.EntityTypeName;
            resourceId = explicitRequest.ResourceId;
            return resourceId > 0;
        }

        var requestType = request.GetType();
        if (ReturnsCollectionResponse(requestType))
            return false;

        if (!TryResolveEntityTypeFromName(requestType.Name, out entityTypeName))
            return false;

        return TryResolveResourceId(requestType, request, out resourceId);
    }

    private bool TryResolveEntityTypeFromName(string typeName, out string entityTypeName)
    {
        entityTypeName = default!;

        if (!typeName.EndsWith("Request", StringComparison.Ordinal))
            return false;

        var stem = typeName[..^"Request".Length];
        foreach (var candidate in entityTypeRegistry.GetRegisteredNamesOrderedByLengthDesc())
        {
            if (!stem.Contains(candidate, StringComparison.Ordinal))
                continue;

            if (!entityTypeRegistry.IsRegistered(candidate))
                continue;

            entityTypeName = candidate;
            return true;
        }

        return false;
    }

    private static bool TryResolveResourceId(Type requestType, object request, out int resourceId)
    {
        foreach (var property in requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.PropertyType != typeof(int))
                continue;

            var name = property.Name;
            if (name is not ("Id" or "ResourceId") && !name.EndsWith("Id", StringComparison.Ordinal))
                continue;

            resourceId = (int)property.GetValue(request)!;
            if (resourceId > 0)
                return true;
        }

        resourceId = 0;
        return false;
    }

    private static bool ReturnsCollectionResponse(Type requestType)
        => CollectionResponseCache.GetOrAdd(requestType, ResolveReturnsCollectionResponse);

    private static bool ResolveReturnsCollectionResponse(Type requestType)
    {
        var responseType = GetResponseType(requestType);
        if (responseType is null)
            return false;

        responseType = UnwrapOperationResult(responseType);
        return IsCollectionType(responseType);
    }

    private static Type? GetResponseType(Type requestType)
    {
        var requestInterface = requestType
            .GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequest<>));

        return requestInterface?.GetGenericArguments()[0];
    }

    private static Type UnwrapOperationResult(Type responseType)
    {
        if (!responseType.IsGenericType)
            return responseType;

        if (responseType.GetGenericTypeDefinition() == typeof(OperationResult<>))
            return responseType.GetGenericArguments()[0];

        return responseType;
    }

    private static bool IsCollectionType(Type type)
    {
        if (type == typeof(string))
            return false;

        if (type.IsGenericType)
        {
            var genericDefinition = type.GetGenericTypeDefinition();
            if (genericDefinition == typeof(PagedResult<>)
                || genericDefinition == typeof(List<>)
                || genericDefinition == typeof(IReadOnlyList<>)
                || genericDefinition == typeof(IEnumerable<>))
                return true;
        }

        return typeof(IEnumerable).IsAssignableFrom(type);
    }
}
