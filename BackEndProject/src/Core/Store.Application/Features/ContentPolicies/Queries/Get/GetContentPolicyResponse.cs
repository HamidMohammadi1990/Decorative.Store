using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Queries;

public record GetContentPolicyRuleSummaryResponse
{
    public int Id { get; init; }
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}

public record GetContentPolicyResponse
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
    public List<GetContentPolicyRuleSummaryResponse> Rules { get; init; } = [];
    public List<int> RecordEntityIds { get; init; } = [];
}
