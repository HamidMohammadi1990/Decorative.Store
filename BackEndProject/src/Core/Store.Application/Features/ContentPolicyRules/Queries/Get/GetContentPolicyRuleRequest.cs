using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyRules.Queries;

public record GetContentPolicyRuleRequest : IRequest<OperationResult<GetContentPolicyRuleResponse?>>
{
    public int Id { get; init; }
}

public record GetContentPolicyRuleResponse
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
