using System.Linq.Expressions;
using Edition.Application.Models.ContentPolicies;
using Microsoft.Extensions.Logging;
using Store.Domain.Common;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyExpressionBuilder
    (ContentPolicyRuleExpressionFactory ruleExpressionFactory, ILogger<ContentPolicyExpressionBuilder> logger)
{
    public Expression<Func<TEntity, bool>>? Build<TEntity>(
        string entityTypeName,
        IReadOnlyList<ContentPolicyWithRulesDto> policies,
        ContentPolicyContext context)
    {
        if (policies.Count == 0)
            return null;

        var entity = Expression.Parameter(typeof(TEntity), "entity");
        Expression? allowOr = null;
        Expression? denyOr = null;

        foreach (var policy in policies.OrderBy(x => x.Priority).ThenBy(x => x.Id))
        {
            var policyExpression = BuildPolicyExpression(entityTypeName, policy, entity, context);
            if (policyExpression is null)
                continue;

            if (policy.Effect == ContentPolicyEffect.Deny)
                denyOr = denyOr is null ? policyExpression : Expression.OrElse(denyOr, policyExpression);
            else
                allowOr = allowOr is null ? policyExpression : Expression.OrElse(allowOr, policyExpression);
        }

        var finalExpression = CombineAllowDeny(allowOr, denyOr);
        if (finalExpression is null)
            return null;

        return Expression.Lambda<Func<TEntity, bool>>(finalExpression, entity);
    }

    private Expression? BuildPolicyExpression(
        string entityTypeName,
        ContentPolicyWithRulesDto policy,
        ParameterExpression entity,
        ContentPolicyContext context)
    {
        Expression? policyOr = null;
        var hasInvalidRule = false;

        foreach (var group in policy.Rules.GroupBy(x => x.RuleGroup).OrderBy(x => x.Key))
        {
            Expression? groupAnd = null;

            foreach (var rule in group.OrderBy(x => x.SortOrder))
            {
                if (!ruleExpressionFactory.TryCreateRuleExpression(
                        entityTypeName,
                        rule,
                        entity,
                        context,
                        out var ruleExpression,
                        out var error))
                {
                    hasInvalidRule = true;
                    logger.LogWarning(
                        "Invalid content policy rule. PolicyId={PolicyId}, FieldPath={FieldPath}, Error={Error}",
                        policy.Id,
                        rule.FieldPath,
                        error);
                    continue;
                }

                groupAnd = groupAnd is null ? ruleExpression : Expression.AndAlso(groupAnd, ruleExpression);
            }

            if (groupAnd is null)
                continue;

            policyOr = policyOr is null ? groupAnd : Expression.OrElse(policyOr, groupAnd);
        }

        var recordAccessExpression = BuildRecordAccessExpression(entity, policy.RecordEntityIds);
        if (recordAccessExpression is not null)
            policyOr = policyOr is null ? recordAccessExpression : Expression.OrElse(policyOr, recordAccessExpression);

        if (policyOr is null && hasInvalidRule && policy.Rules.Count > 0 && policy.RecordEntityIds.Count == 0)
            return Expression.Constant(false);

        return policyOr;
    }

    private static Expression? BuildRecordAccessExpression(
        ParameterExpression entity,
        IReadOnlyList<int> recordEntityIds)
    {
        if (recordEntityIds.Count == 0)
            return null;

        var idProperty = entity.Type.GetProperty(nameof(IEntity.Id));
        if (idProperty is null)
            return null;

        Expression? recordOr = null;
        foreach (var entityId in recordEntityIds.Distinct())
        {
            var idEquals = Expression.Equal(
                Expression.Property(entity, idProperty),
                Expression.Constant(entityId, idProperty.PropertyType));
            recordOr = recordOr is null ? idEquals : Expression.OrElse(recordOr, idEquals);
        }

        return recordOr;
    }

    private static Expression? CombineAllowDeny(Expression? allowOr, Expression? denyOr)
    {
        if (allowOr is null && denyOr is null)
            return null;

        if (allowOr is null)
            return Expression.Not(denyOr!);

        if (denyOr is null)
            return allowOr;

        return Expression.AndAlso(allowOr, Expression.Not(denyOr));
    }
}
