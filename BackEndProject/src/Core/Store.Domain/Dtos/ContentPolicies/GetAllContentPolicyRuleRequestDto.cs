using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.ContentPolicies;

public record GetAllContentPolicyRuleRequestDto
{
    [QueryFilter]
    public int? PolicyId { get; init; }

    [QueryFilter(MemberPath = "Policy.EntityType")]
    public string? EntityType { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? FieldPath { get; init; }

    [QueryFilter]
    public ContentPolicyOperator? Operator { get; init; }

    [QueryFilter]
    public ContentPolicyValueType? ValueType { get; init; }

    [QueryFilter]
    public ContentPolicyRuleGroup? RuleGroup { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
