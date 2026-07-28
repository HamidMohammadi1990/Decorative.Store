using System.Collections;
using System.Linq.Expressions;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.Enums;
using Store.Domain.Dtos.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

public static class ContentPolicyOperatorApplier
{
    public static Expression? Apply(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        if (@operator is ContentPolicyOperator.IsNull or ContentPolicyOperator.IsNotNull)
            return ApplyNullOperator(@operator, memberExpression, memberType);

        if (ContentPolicyOperatorKinds.IsBetweenOperator(@operator))
            return ApplyBetweenOperator(@operator, memberExpression, memberType, rule, context);

        if (@operator is ContentPolicyOperator.In or ContentPolicyOperator.NotIn)
            return ApplyInOperator(@operator, memberExpression, memberType, rule, context);

        if (@operator is ContentPolicyOperator.Exists or ContentPolicyOperator.NotExists)
            return ApplyExistsOperator(@operator, memberExpression, memberType);

        return ApplyComparisonOperator(@operator, memberExpression, memberType, rule, context);
    }

    public static Expression? ApplyCollectionCount(
        ContentPolicyOperator @operator,
        Expression collectionExpression,
        Type collectionType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        if (!ContentPolicyOperatorKinds.IsCountOperator(@operator))
            return null;

        var elementType = GetEnumerableElementType(collectionType);
        if (elementType is null)
            return null;

        var countCall = Expression.Call(
            typeof(Enumerable),
            nameof(Enumerable.Count),
            [elementType],
            collectionExpression);

        var resolved = ContentPolicyValueResolver.ResolveScalar(rule.ValueType, rule.Value, typeof(int), context);
        if (resolved is null)
            return null;

        var countValue = Convert.ToInt32(resolved);
        var constant = Expression.Constant(countValue);

        return @operator switch
        {
            ContentPolicyOperator.CountEquals => Expression.Equal(countCall, constant),
            ContentPolicyOperator.CountNotEquals => Expression.NotEqual(countCall, constant),
            ContentPolicyOperator.CountGreaterThan => Expression.GreaterThan(countCall, constant),
            ContentPolicyOperator.CountGreaterThanOrEqual => Expression.GreaterThanOrEqual(countCall, constant),
            ContentPolicyOperator.CountLessThan => Expression.LessThan(countCall, constant),
            ContentPolicyOperator.CountLessThanOrEqual => Expression.LessThanOrEqual(countCall, constant),
            _ => null
        };
    }

    private static Expression? ApplyBetweenOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        var bounds = ContentPolicyValueResolver.ResolveBetweenBounds(rule.ValueType, rule.Value, memberType, context);
        if (bounds is null)
            return null;

        var (min, max) = bounds.Value;
        var constantType = memberType;

        if (Nullable.GetUnderlyingType(memberType) is { } underlyingType)
        {
            min = Convert.ChangeType(min, underlyingType);
            max = Convert.ChangeType(max, underlyingType);
        }

        var minConstant = Expression.Constant(min, constantType);
        var maxConstant = Expression.Constant(max, constantType);
        var lowerBound = Expression.GreaterThanOrEqual(memberExpression, minConstant);
        var upperBound = Expression.LessThanOrEqual(memberExpression, maxConstant);
        var between = Expression.AndAlso(lowerBound, upperBound);

        return @operator == ContentPolicyOperator.Between ? between : Expression.Not(between);
    }

    private static Expression? ApplyExistsOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType)
    {
        if (memberType == typeof(bool))
            return @operator == ContentPolicyOperator.Exists ? memberExpression : Expression.Not(memberExpression);

        if (memberType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(memberType))
        {
            var elementType = GetEnumerableElementType(memberType);
            if (elementType is null)
                return null;

            var anyCall = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Any),
                [elementType],
                memberExpression);

            return @operator == ContentPolicyOperator.Exists ? anyCall : Expression.Not(anyCall);
        }

        if (memberType != typeof(string) && memberType.IsClass)
        {
            var nullConstant = Expression.Constant(null, memberType);
            return @operator == ContentPolicyOperator.Exists
                ? Expression.NotEqual(memberExpression, nullConstant)
                : Expression.Equal(memberExpression, nullConstant);
        }

        return null;
    }

    internal static bool ShouldNegateCollectionExistence(ContentPolicyOperator @operator)
        => @operator is ContentPolicyOperator.NotEquals
            or ContentPolicyOperator.NotIn
            or ContentPolicyOperator.NotContains;

    internal static bool TryGetPositiveExistenceOperator(
        ContentPolicyOperator @operator,
        out ContentPolicyOperator positiveOperator)
    {
        switch (@operator)
        {
            case ContentPolicyOperator.NotEquals:
                positiveOperator = ContentPolicyOperator.Equals;
                return true;
            case ContentPolicyOperator.NotIn:
                positiveOperator = ContentPolicyOperator.In;
                return true;
            case ContentPolicyOperator.NotContains:
                positiveOperator = ContentPolicyOperator.Contains;
                return true;
            default:
                positiveOperator = default;
                return false;
        }
    }

    private static Expression ApplyNullOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType)
    {
        var underlying = Nullable.GetUnderlyingType(memberType) ?? memberType;
        if (underlying.IsValueType && Nullable.GetUnderlyingType(memberType) is null)
            return Expression.Constant(@operator == ContentPolicyOperator.IsNotNull);

        var nullConstant = Expression.Constant(null, memberType);
        return @operator == ContentPolicyOperator.IsNull
            ? Expression.Equal(memberExpression, nullConstant)
            : Expression.NotEqual(memberExpression, nullConstant);
    }

    private static Expression? ApplyInOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        var intList = ContentPolicyValueResolver.ResolveIntList(rule.ValueType, rule.Value, context);
        if (intList is not null && IsIntCompatible(memberType))
            return ApplyContains(@operator, intList, memberExpression, typeof(int));

        if (rule.ValueType == ContentPolicyValueType.Context)
        {
            var contextCollection = ContentPolicyValueResolver.ResolveContextCollection(rule.Value, context);
            var containsExpression = TryBuildContainsFromCollection(@operator, contextCollection, memberExpression, memberType);
            if (containsExpression is not null)
                return containsExpression;
        }

        if (rule.ValueType == ContentPolicyValueType.Literal)
        {
            var values = rule.Value
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => ContentPolicyValueResolver.ResolveScalar(ContentPolicyValueType.Literal, x, memberType, context))
                .Where(x => x is not null)
                .ToArray();

            if (values.Length == 0)
                return null;

            Expression? orExpression = null;
            foreach (var value in values)
            {
                var literal = Expression.Constant(value, memberType);
                var equals = Expression.Equal(memberExpression, literal);
                orExpression = orExpression is null ? equals : Expression.OrElse(orExpression, equals);
            }

            if (orExpression is null)
                return null;

            return @operator == ContentPolicyOperator.In ? orExpression : Expression.Not(orExpression);
        }

        return null;
    }

    private static Expression? TryBuildContainsFromCollection(
        ContentPolicyOperator @operator,
        object? contextCollection,
        Expression memberExpression,
        Type memberType)
    {
        if (contextCollection is null)
            return null;

        if (contextCollection is string)
            return null;

        if (contextCollection is not IEnumerable enumerable)
            return null;

        var values = enumerable.Cast<object?>().Where(x => x is not null).ToArray();
        if (values.Length == 0)
            return null;

        var elementType = values[0]!.GetType();
        if (!memberType.IsAssignableFrom(elementType) && Nullable.GetUnderlyingType(memberType) != elementType)
            return null;

        var typedArray = Array.CreateInstance(elementType, values.Length);
        for (var i = 0; i < values.Length; i++)
            typedArray.SetValue(values[i], i);

        return ApplyContains(@operator, typedArray, memberExpression, elementType);
    }

    private static Expression ApplyContains(
        ContentPolicyOperator @operator,
        object collection,
        Expression memberExpression,
        Type elementType)
    {
        var contains = Expression.Call(
            typeof(Enumerable),
            nameof(Enumerable.Contains),
            [elementType],
            Expression.Constant(collection),
            memberExpression);

        return @operator == ContentPolicyOperator.In ? contains : Expression.Not(contains);
    }

    private static Expression? ApplyComparisonOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        if (@operator is ContentPolicyOperator.Contains or ContentPolicyOperator.NotContains
            or ContentPolicyOperator.StartsWith or ContentPolicyOperator.EndsWith)
            return ApplyStringOperator(@operator, memberExpression, memberType, rule, context);

        var resolved = ContentPolicyValueResolver.ResolveScalar(rule.ValueType, rule.Value, memberType, context);
        if (resolved is null)
            return null;

        var constantType = memberType;
        if (Nullable.GetUnderlyingType(memberType) is { } underlyingType)
        {
            resolved = Convert.ChangeType(resolved, underlyingType);
            constantType = memberType;
        }

        var constant = Expression.Constant(resolved, constantType);
        return @operator switch
        {
            ContentPolicyOperator.Equals => Expression.Equal(memberExpression, constant),
            ContentPolicyOperator.NotEquals => Expression.NotEqual(memberExpression, constant),
            ContentPolicyOperator.GreaterThan => Expression.GreaterThan(memberExpression, constant),
            ContentPolicyOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberExpression, constant),
            ContentPolicyOperator.LessThan => Expression.LessThan(memberExpression, constant),
            ContentPolicyOperator.LessThanOrEqual => Expression.LessThanOrEqual(memberExpression, constant),
            _ => null
        };
    }

    private static Expression? ApplyStringOperator(
        ContentPolicyOperator @operator,
        Expression memberExpression,
        Type memberType,
        ContentPolicyRuleDto rule,
        ContentPolicyContext context)
    {
        if (memberType != typeof(string))
            return null;

        var resolved = ContentPolicyValueResolver.ResolveScalar(rule.ValueType, rule.Value, memberType, context) as string;
        if (string.IsNullOrEmpty(resolved))
            return null;

        var methodName = @operator switch
        {
            ContentPolicyOperator.Contains or ContentPolicyOperator.NotContains => nameof(string.Contains),
            ContentPolicyOperator.StartsWith => nameof(string.StartsWith),
            ContentPolicyOperator.EndsWith => nameof(string.EndsWith),
            _ => null
        };

        if (methodName is null)
            return null;

        var method = typeof(string).GetMethod(methodName, [typeof(string)]);
        if (method is null)
            return null;

        var call = Expression.Call(memberExpression, method, Expression.Constant(resolved));
        return @operator == ContentPolicyOperator.NotContains ? Expression.Not(call) : call;
    }

    private static bool IsIntCompatible(Type memberType)
        => memberType == typeof(int) || Nullable.GetUnderlyingType(memberType) == typeof(int);

    private static Type? GetEnumerableElementType(Type collectionType)
    {
        if (collectionType.IsArray)
            return collectionType.GetElementType();

        if (collectionType.IsGenericType)
            return collectionType.GetGenericArguments()[0];

        return collectionType.GetInterfaces()
            .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }
}
