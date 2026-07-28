using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Queries;

public record GetAllContentPolicyResponse
{
    public int Id { get; init; }
    public ContentPolicyScope Scope { get; init; }
    public ContentPolicyMergeMode MergeMode { get; init; }
    public int? RoleId { get; init; }
    public int? UserId { get; init; }
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.All;
    public string Name { get; init; } = default!;
    public ContentPolicyEffect Effect { get; init; }
    public bool IsActive { get; init; }
    public int Priority { get; init; }
    public int RuleCount { get; init; }
}
