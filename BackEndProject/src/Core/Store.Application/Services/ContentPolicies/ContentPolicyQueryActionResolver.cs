using Store.Domain.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public static class ContentPolicyQueryActionResolver
{
    public static ContentPolicyQueryAction Resolve(Type requestType)
    {
        var typeName = requestType.Name;
        if (!typeName.EndsWith("Request", StringComparison.Ordinal))
            return ContentPolicyQueryAction.All;

        var stem = typeName[..^"Request".Length];

        if (stem.StartsWith("GetAll", StringComparison.Ordinal))
            return ContentPolicyQueryAction.GetAll;

        if (stem.StartsWith("Search", StringComparison.Ordinal))
            return ContentPolicyQueryAction.Search;

        if (string.Equals(stem, "GetUserAddresses", StringComparison.Ordinal))
            return ContentPolicyQueryAction.GetUserAddresses;

        if (stem.StartsWith("Get", StringComparison.Ordinal))
            return ContentPolicyQueryAction.Get;

        return ContentPolicyQueryAction.All;
    }

    public static ContentPolicyQueryAction Resolve(object request)
        => Resolve(request.GetType());
}
