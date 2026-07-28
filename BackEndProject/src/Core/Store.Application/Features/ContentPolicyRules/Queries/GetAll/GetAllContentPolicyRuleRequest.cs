using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ContentPolicyRules.Queries;

public record GetAllContentPolicyRuleRequest : IRequest<OperationResult<PagedResult<GetAllContentPolicyRuleResponse>>>
{
    public int? PolicyId { get; init; }
    public string? EntityType { get; init; }
    public string? FieldPath { get; init; }
    public ContentPolicyOperator? Operator { get; init; }
    public ContentPolicyValueType? ValueType { get; init; }
    public ContentPolicyRuleGroup? RuleGroup { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllContentPolicyRuleResponse
{
    public int Id { get; init; }
    public int PolicyId { get; init; }
    public string EntityType { get; init; }
    public string PolicyName { get; init; } = default!;
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}
