namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyResourceRequestResolver
{
    bool TryResolve(object request, out string entityTypeName, out int resourceId);
}
