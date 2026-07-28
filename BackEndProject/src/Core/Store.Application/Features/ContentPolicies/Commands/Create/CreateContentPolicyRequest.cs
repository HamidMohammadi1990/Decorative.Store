using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Commands;

public record CreateContentPolicyRuleItemRequest
{
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}

public record CreateContentPolicyRequest : IRequest<OperationResult<CreateContentPolicyResponse>>
{
    public int? RoleId { get; init; }
    public int? UserId { get; init; }
    public ContentPolicyMergeMode MergeMode { get; init; } = ContentPolicyMergeMode.Additive;
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.All;
    public string Name { get; init; } = default!;
    public ContentPolicyEffect Effect { get; init; } = ContentPolicyEffect.Allow;
    public bool IsActive { get; init; } = true;
    public int Priority { get; init; }
    public List<CreateContentPolicyRuleItemRequest> Rules { get; init; } = [];
}

public record CreateContentPolicyResponse
{
    public int Id { get; init; }
}
