using Edition.Application.Contracts.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyRecordAccessValidator(IContentPolicyEntityAccessQuery entityAccessQuery)
{
    public async Task<string?> ValidateEntityExistsAsync(
        string entityType,
        int entityId,
        CancellationToken cancellationToken = default)
    {
        if (entityId <= 0)
            return "Entity id must be greater than zero.";

        var exists = await entityAccessQuery.ExistsAsync(entityType, entityId, null, cancellationToken);
        return exists ? null : $"Entity '{entityType}' with id {entityId} was not found.";
    }
}
