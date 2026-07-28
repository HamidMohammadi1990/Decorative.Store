namespace Edition.Application.Contracts.ContentPolicies;

public interface IContentPolicyAccessChecker
{
    Task EnsureAccessibleAsync(
        string entityType,
        int resourceId,
        CancellationToken cancellationToken = default);
}
