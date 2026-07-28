using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public record ValidateContentPolicyRuleInput
{
    public string FieldPath { get; init; } = default!;
    public ContentPolicyOperator Operator { get; init; }
    public ContentPolicyValueType ValueType { get; init; }
    public string Value { get; init; } = default!;
    public int SortOrder { get; init; }
    public ContentPolicyRuleGroup RuleGroup { get; init; }
}

public record ValidateContentPolicyRulesRequest : IRequest<OperationResult<ValidateContentPolicyRulesResponse>>
{
    public string EntityType { get; init; }
    public List<ValidateContentPolicyRuleInput> Rules { get; init; } = [];
}
