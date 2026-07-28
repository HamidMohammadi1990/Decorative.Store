using Store.Domain.ContentPolicies;
using Store.Domain.Common;

namespace Store.Domain.Entities;

[ExcludeFromContentPolicy]
public class ContentPolicyRecordAccess : BaseEntity
{
    public int PolicyId { get; private set; }
    public int EntityId { get; private set; }

    public ContentPolicy Policy { get; private set; } = default!;

    public static ContentPolicyRecordAccess Create(int policyId, int entityId)
        => new() { PolicyId = policyId, EntityId = entityId };

    public static ContentPolicyRecordAccess CreateForPolicy(int policyId, int entityId)
        => Create(policyId, entityId);
}
