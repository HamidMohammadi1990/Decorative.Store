using System.Linq.Expressions;
using System.Reflection;
using Store.Domain.Enums;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyRuleExpressionFactory
    (IContentEntityTypeRegistry entityTypeRegistry)
{
    public bool TryCreateRuleExpression(
        string entityTypeName,
        ContentPolicyRuleDto rule,
        ParameterExpression entityParameter,
        ContentPolicyContext context,
        out Expression? expression,
        out string? error)
    {
        expression = null;
        error = null;

        if (!ValidatePathPrefix(entityTypeName, rule.FieldPath, out var segments, out error))
            return false;

        var memberSegments = segments[1..];
        if (memberSegments.Length == 0)
        {
            error = $"Field path '{rule.FieldPath}' must include at least one member.";
            return false;
        }

        if (memberSegments.Length > ContentPolicyTypeReflection.MaxPathDepth)
        {
            error = $"Field path '{rule.FieldPath}' exceeds the maximum depth of {ContentPolicyTypeReflection.MaxPathDepth}.";
            return false;
        }

        if (!TryBuildFilterExpression(
                entityParameter,
                entityParameter.Type,
                memberSegments,
                rule,
                context,
                depth: 0,
                out expression,
                out error))
            return false;

        if (expression is null || expression.Type != typeof(bool))
        {
            error = $"Field path '{rule.FieldPath}' must resolve to a boolean filter.";
            expression = null;
            return false;
        }

        return true;
    }

    private bool ValidatePathPrefix(
        string entityTypeName,
        string fieldPath,
        out string[] segments,
        out string? error)
    {
        error = null;
        segments = fieldPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (segments.Length < 2)
        {
            error = $"Field path '{fieldPath}' must include entity prefix and at least one member.";
            return false;
        }

        var expectedPrefix = entityTypeRegistry.GetEntityPrefix(entityTypeName);
        if (!string.Equals(segments[0], expectedPrefix, StringComparison.Ordinal))
        {
            error = $"Field path '{fieldPath}' must start with '{expectedPrefix}'.";
            return false;
        }

        return true;
    }

    private static bool TryBuildFilterExpression(
        Expression currentExpression,
        Type currentType,
        string[] segments,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context,
        int depth,
        out Expression? expression,
        out string? error)
    {
        expression = null;
        error = null;

        if (segments.Length == 0)
        {
            error = "Field path must include at least one member.";
            return false;
        }

        var segment = segments[0];
        var remaining = segments[1..];

        if (!TryGetProperty(currentType, segment, out var property))
        {
            error = $"Property '{segment}' was not found on '{currentType.Name}'.";
            return false;
        }

        var memberExpression = Expression.Property(currentExpression, property);
        var memberType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        if (ContentPolicyTypeReflection.IsCollection(memberType))
        {
            if (remaining.Length == 0)
            {
                if (ContentPolicyOperatorKinds.IsCountOperator(rule.Operator))
                {
                    expression = ContentPolicyOperatorApplier.ApplyCollectionCount(
                        rule.Operator,
                        memberExpression,
                        property.PropertyType,
                        rule,
                        context);

                    if (expression is null)
                        error = $"Operator '{rule.Operator}' requires a numeric count value for collection '{segment}'.";

                    return expression is not null;
                }

                expression = ContentPolicyOperatorApplier.Apply(
                    rule.Operator,
                    memberExpression,
                    property.PropertyType,
                    rule,
                    context);

                if (expression is null)
                    error = $"Operator '{rule.Operator}' is not supported for collection '{segment}'.";

                return expression is not null;
            }

            return TryBuildCollectionAny(
                memberExpression,
                property.PropertyType,
                remaining,
                rule,
                context,
                depth + 1,
                out expression,
                out error);
        }

        if (remaining.Length == 0)
        {
            expression = ContentPolicyOperatorApplier.Apply(
                rule.Operator,
                memberExpression,
                property.PropertyType,
                rule,
                context);

            if (expression is null)
                error = $"Operator '{rule.Operator}' is not supported for '{segment}'.";

            return expression is not null;
        }

        if (!ContentPolicyTypeReflection.IsNavigation(property.PropertyType))
        {
            error = $"Cannot traverse into '{segment}' because it is not a navigation property.";
            return false;
        }

        return TryBuildFilterExpression(
            memberExpression,
            memberType,
            remaining,
            rule,
            context,
            depth + 1,
            out expression,
            out error);
    }

    private static bool TryBuildCollectionAny(
        Expression collectionExpression,
        Type collectionType,
        string[] remainingSegments,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context,
        int depth,
        out Expression? expression,
        out string? error)
    {
        expression = null;
        error = null;

        var elementType = ContentPolicyTypeReflection.GetCollectionElementType(collectionType);
        if (elementType is null)
        {
            error = "Unable to resolve the collection element type.";
            return false;
        }

        var elementParameter = Expression.Parameter(elementType, $"cpItem{depth}");
        var effectiveRule = rule;
        var negateExistence = ContentPolicyOperatorApplier.ShouldNegateCollectionExistence(rule.Operator);

        if (negateExistence)
        {
            if (!ContentPolicyOperatorApplier.TryGetPositiveExistenceOperator(rule.Operator, out var positiveOperator))
            {
                error = $"Operator '{rule.Operator}' is not supported inside collection paths.";
                return false;
            }

            effectiveRule = rule with { Operator = positiveOperator };
        }

        if (!TryBuildFilterExpression(
                elementParameter,
                elementType,
                remainingSegments,
                effectiveRule,
                context,
                depth + 1,
                out var predicate,
                out error))
            return false;

        if (predicate is null || predicate.Type != typeof(bool))
        {
            error = "Collection filters must resolve to a boolean predicate.";
            return false;
        }

        var anyCall = Expression.Call(
            typeof(Enumerable),
            nameof(Enumerable.Any),
            [elementType],
            collectionExpression,
            Expression.Lambda(predicate, elementParameter));

        expression = negateExistence ? Expression.Not(anyCall) : anyCall;
        return true;
    }

    private static bool TryGetProperty(Type type, string name, out PropertyInfo property)
    {
        property = ContentPolicyPropertyReflectionCache.GetProperty(type, name)!;
        return property is not null;
    }
}
