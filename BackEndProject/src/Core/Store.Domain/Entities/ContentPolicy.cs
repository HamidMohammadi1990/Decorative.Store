using Edition.Domain.ContentPolicies;
using Store.Domain.Enums;
using Store.Domain.Common;

namespace Store.Domain.Entities;

[ExcludeFromContentPolicy]
public class ContentPolicy : BaseEntity
{
    public int? RoleId { get; private set; }
    public int? UserId { get; private set; }
    public string EntityType { get; private set; } = default!;
    public ContentPolicyQueryAction QueryAction { get; private set; } = ContentPolicyQueryAction.All;
    public string Name { get; private set; } = default!;
    public ContentPolicyEffect Effect { get; private set; } = ContentPolicyEffect.Allow;
    public ContentPolicyMergeMode MergeMode { get; private set; } = ContentPolicyMergeMode.Additive;
    public bool IsActive { get; private set; } = true;
    public int Priority { get; private set; }

    public Role? Role { get; private set; }
    public User? User { get; private set; }
    public ICollection<ContentPolicyRule> Rules { get; private set; } = [];
    public ICollection<ContentPolicyRecordAccess> RecordAccesses { get; private set; } = [];

    public ContentPolicyScope Scope
        => UserId.HasValue ? ContentPolicyScope.User : ContentPolicyScope.Role;

    public static ContentPolicy Create(
        int? roleId,
        int? userId,
        string entityType,
        string name,
        int priority = 0,
        ContentPolicyEffect effect = ContentPolicyEffect.Allow,
        ContentPolicyQueryAction queryAction = ContentPolicyQueryAction.All,
        ContentPolicyMergeMode mergeMode = ContentPolicyMergeMode.Additive)
        => new()
        {
            Name = name,
            RoleId = roleId,
            UserId = userId,
            Effect = effect,
            Priority = priority,
            EntityType = entityType,
            MergeMode = NormalizeMergeMode(roleId, userId, mergeMode),
            QueryAction = queryAction
        };

    public void UpdateScope(int? roleId, int? userId, ContentPolicyMergeMode mergeMode)
    {
        RoleId = roleId;
        UserId = userId;
        MergeMode = NormalizeMergeMode(roleId, userId, mergeMode);
    }

    public void Update(
        string name,
        ContentPolicyEffect effect,
        bool isActive,
        int priority,
        ContentPolicyQueryAction queryAction = ContentPolicyQueryAction.All)
    {
        Name = name;
        Effect = effect;
        IsActive = isActive;
        Priority = priority;
        QueryAction = queryAction;
    }

    public void AddRules(params ContentPolicyRule[] rules)
    {
        foreach (var rule in rules)
            Rules.Add(rule);
    }

    public void ReplaceRules(IEnumerable<ContentPolicyRule> rules)
    {
        Rules.Clear();
        foreach (var rule in rules)
            Rules.Add(rule);
    }

    private static ContentPolicyMergeMode NormalizeMergeMode(
        int? roleId,
        int? userId,
        ContentPolicyMergeMode mergeMode)
        => userId is > 0 ? mergeMode : ContentPolicyMergeMode.Additive;
}
