using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using Store.Domain.QueryFilters;

namespace Store.Infrastructure.Persistence.Extensions;

public static class QueryableFilterExtensions
{
    private sealed record FilterRule(
        PropertyInfo RequestProperty,
        QueryFilterAttribute Attribute,
        string MemberPath);

    private static readonly ConcurrentDictionary<Type, FilterRule[]> FilterRuleCache = new();

    public static IQueryable<T> ApplyQueryFilters<T, TFilter>(this IQueryable<T> query, TFilter? filter)
        where T : class
    {
        if (filter is null)
            return query;

        var rules = FilterRuleCache.GetOrAdd(typeof(TFilter), BuildFilterRules);

        foreach (var rule in rules)
        {
            var value = rule.RequestProperty.GetValue(filter);
            if (!ShouldApplyFilter(value, rule.Attribute, rule.RequestProperty.PropertyType))
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = BuildMemberAccess(parameter, rule.MemberPath);
            var body = BuildFilterExpression(member, value, rule.Attribute);
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            query = query.Where(predicate);
        }

        return query;
    }

    private static FilterRule[] BuildFilterRules(Type filterType) =>
        [.. filterType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => (Property: property, Attribute: property.GetCustomAttribute<QueryFilterAttribute>()))
            .Where(x => x.Attribute is not null)
            .Select(x => new FilterRule(
                x.Property,
                x.Attribute!,
                x.Attribute!.MemberPath ?? x.Property.Name))];

    private static bool ShouldApplyFilter(object? value, QueryFilterAttribute attribute, Type propertyType)
    {
        if (attribute.Operator is FilterOperator.IsNull or FilterOperator.IsNotNull)
            return true;

        if (value is null)
            return !attribute.IgnoreWhenNull;

        if (propertyType == typeof(string))
        {
            if (attribute.IgnoreWhenEmpty && string.IsNullOrWhiteSpace((string)value))
                return false;

            return true;
        }

        if (attribute.IgnoreWhenDefault && IsDefaultValue(value, propertyType))
            return false;

        return true;
    }

    private static bool IsDefaultValue(object value, Type propertyType)
    {
        var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (!targetType.IsValueType)
            return false;

        return Equals(value, Activator.CreateInstance(targetType));
    }

    private static Expression BuildMemberAccess(Expression root, string memberPath)
    {
        Expression current = root;

        foreach (var segment in memberPath.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            current = Expression.PropertyOrField(current, segment);

        return current;
    }

    private static Expression BuildFilterExpression(
        Expression member,
        object? value,
        QueryFilterAttribute attribute)
    {
        return attribute.Operator switch
        {
            FilterOperator.Equal => BuildEquality(member, value, equal: true),
            FilterOperator.NotEqual => BuildEquality(member, value, equal: false),
            FilterOperator.GreaterThan => BuildComparison(member, value, ExpressionType.GreaterThan),
            FilterOperator.LessThan => BuildComparison(member, value, ExpressionType.LessThan),
            FilterOperator.GreaterThanOrEqual => BuildComparison(member, value, ExpressionType.GreaterThanOrEqual),
            FilterOperator.LessThanOrEqual => BuildComparison(member, value, ExpressionType.LessThanOrEqual),
            FilterOperator.Contains => BuildStringMethod(member, value, nameof(string.Contains), [typeof(string)]),
            FilterOperator.NotContains => Expression.Not(BuildStringMethod(member, value, nameof(string.Contains), [typeof(string)])),
            FilterOperator.StartsWith => BuildStringMethod(member, value, nameof(string.StartsWith), [typeof(string)]),
            FilterOperator.EndsWith => BuildStringMethod(member, value, nameof(string.EndsWith), [typeof(string)]),
            FilterOperator.IsNull => Expression.Equal(member, Expression.Constant(null, member.Type)),
            FilterOperator.IsNotNull => Expression.NotEqual(member, Expression.Constant(null, member.Type)),
            _ => throw new NotSupportedException($"Filter operator '{attribute.Operator}' is not supported.")
        };
    }

    private static Expression BuildEquality(Expression member, object? value, bool equal)
    {
        var constant = Expression.Constant(NormalizeFilterValue(value, member.Type), member.Type);
        return equal ? Expression.Equal(member, constant) : Expression.NotEqual(member, constant);
    }

    private static Expression BuildComparison(Expression member, object? value, ExpressionType comparisonType)
    {
        var constant = Expression.Constant(NormalizeFilterValue(value, member.Type), member.Type);
        return Expression.MakeBinary(comparisonType, member, constant);
    }

    private static Expression BuildStringMethod(
        Expression member,
        object? value,
        string methodName,
        Type[] parameterTypes)
    {
        if (value is not string stringValue)
            throw new InvalidOperationException($"String filter operator '{methodName}' requires a string value.");

        var method = typeof(string).GetMethod(methodName, parameterTypes)
            ?? throw new InvalidOperationException($"String method '{methodName}' was not found.");

        return Expression.Call(member, method, Expression.Constant(stringValue));
    }

    private static object? NormalizeFilterValue(object? value, Type memberType)
    {
        if (value is null)
            return null;

        var targetType = Nullable.GetUnderlyingType(memberType) ?? memberType;

        if (targetType.IsEnum && value.GetType() != targetType)
            return Enum.ToObject(targetType, value);

        if (targetType.IsInstanceOfType(value))
            return value;

        return Convert.ChangeType(value, targetType);
    }
}