using Edition.Domain.ContentPolicies;
using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

[ExcludeFromContentPolicy]
public class ContentPolicyRule : BaseEntity
{
    public int PolicyId { get; private set; }
    public ContentPolicyRuleGroup RuleGroup { get; private set; } = ContentPolicyRuleGroup.Group0;
    public string FieldPath { get; private set; } = default!;
    public ContentPolicyOperator Operator { get; private set; }
    public ContentPolicyValueType ValueType { get; private set; }
    public string Value { get; private set; } = default!;
    public int SortOrder { get; private set; }

    public ContentPolicy Policy { get; private set; } = default!;

    public static ContentPolicyRule Create(
        string fieldPath,
        ContentPolicyOperator @operator,
        ContentPolicyValueType valueType,
        string value,
        int sortOrder = 0,
        ContentPolicyRuleGroup ruleGroup = ContentPolicyRuleGroup.Group0)
        => new()
        {
            Value = value,
            FieldPath = fieldPath,
            SortOrder = sortOrder,
            RuleGroup = ruleGroup,
            Operator = @operator,
            ValueType = valueType
        };

    public static ContentPolicyRule CreateForPolicy(
        int policyId,
        string fieldPath,
        ContentPolicyOperator @operator,
        ContentPolicyValueType valueType,
        string value,
        int sortOrder = 0,
        ContentPolicyRuleGroup ruleGroup = ContentPolicyRuleGroup.Group0)
        => new()
        {
            PolicyId = policyId,
            Value = value,
            FieldPath = fieldPath,
            SortOrder = sortOrder,
            RuleGroup = ruleGroup,
            Operator = @operator,
            ValueType = valueType
        };

    public void Update(
        string fieldPath,
        ContentPolicyOperator @operator,
        ContentPolicyValueType valueType,
        string value,
        int sortOrder,
        ContentPolicyRuleGroup ruleGroup)
    {
        Value = value;
        FieldPath = fieldPath;
        SortOrder = sortOrder;
        RuleGroup = ruleGroup;
        Operator = @operator;
        ValueType = valueType;
    }
}
