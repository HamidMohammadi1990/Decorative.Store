using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyRules.Commands;

public record CreateContentPolicyRuleRequest : IRequest<OperationResult<CreateContentPolicyRuleResponse>>
{
    public int PolicyId { get; init; }
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}

public record CreateContentPolicyRuleResponse
{
    public int Id { get; init; }
}
