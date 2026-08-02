using Store.Domain.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyRuleValidator
    (ContentPolicyRuleExpressionFactory ruleExpressionFactory, IContentEntityTypeRegistry entityTypeRegistry)
{
    public IReadOnlyList<string> ValidateRules(string entityType, IEnumerable<ContentPolicyRuleDto> rules)
    {
        var errors = new List<string>();
        if (!entityTypeRegistry.IsRegistered(entityType))
        {
            errors.Add($"Entity type '{entityType}' is not supported.");
            return errors;
        }

        var entityClrType = entityTypeRegistry.GetClrType(entityType);
        var entityParameter = System.Linq.Expressions.Expression.Parameter(entityClrType, "entity");
        var context = new Models.ContentPolicies.ContentPolicyContext(1, [1, 2]);

        foreach (var rule in rules)
        {
            if (rule.ValueType == ContentPolicyValueType.Context && !ContentPolicyValueResolver.IsValidContextPath(rule.Value))
            {
                errors.Add($"Context path '{rule.Value}' is not supported.");
                continue;
            }

            if (!ruleExpressionFactory.TryCreateRuleExpression(
                    entityType,
                    rule,
                    entityParameter,
                    context,
                    out _,
                    out var error)
                && !string.IsNullOrWhiteSpace(error))
                errors.Add(error);
        }

        return errors;
    }
}
