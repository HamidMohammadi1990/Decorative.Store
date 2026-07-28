using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicies.Commands;

public record UpdateContentPolicyRuleItemRequest
{
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}

public record UpdateContentPolicyRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
    public int? RoleId { get; init; }
    public int? UserId { get; init; }
    public ContentPolicyMergeMode MergeMode { get; init; } = ContentPolicyMergeMode.Additive;
    public string EntityType { get; init; }
    public ContentPolicyQueryAction QueryAction { get; init; } = ContentPolicyQueryAction.All;
    public string Name { get; init; } = default!;
    public ContentPolicyEffect Effect { get; init; }
    public bool IsActive { get; init; }
    public int Priority { get; init; }
    public List<UpdateContentPolicyRuleItemRequest> Rules { get; init; } = [];
}
