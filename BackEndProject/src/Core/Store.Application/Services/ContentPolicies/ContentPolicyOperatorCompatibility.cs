using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public static class ContentPolicyOperatorCompatibility
{
    private static readonly ContentPolicyOperator[] NullOperators =
    [
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    private static readonly ContentPolicyOperator[] EqualityOperators =
    [
        ContentPolicyOperator.Equals,
        ContentPolicyOperator.NotEquals,
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    private static readonly ContentPolicyOperator[] StringOperators =
    [
        ContentPolicyOperator.Equals,
        ContentPolicyOperator.NotEquals,
        ContentPolicyOperator.Contains,
        ContentPolicyOperator.NotContains,
        ContentPolicyOperator.StartsWith,
        ContentPolicyOperator.EndsWith,
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    private static readonly ContentPolicyOperator[] ComparableOperators =
    [
        ContentPolicyOperator.Equals,
        ContentPolicyOperator.NotEquals,
        ContentPolicyOperator.GreaterThan,
        ContentPolicyOperator.GreaterThanOrEqual,
        ContentPolicyOperator.LessThan,
        ContentPolicyOperator.LessThanOrEqual,
        ContentPolicyOperator.Between,
        ContentPolicyOperator.NotBetween,
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    private static readonly ContentPolicyOperator[] IntOperators =
    [
        ContentPolicyOperator.Equals,
        ContentPolicyOperator.NotEquals,
        ContentPolicyOperator.In,
        ContentPolicyOperator.NotIn,
        ContentPolicyOperator.GreaterThan,
        ContentPolicyOperator.GreaterThanOrEqual,
        ContentPolicyOperator.LessThan,
        ContentPolicyOperator.LessThanOrEqual,
        ContentPolicyOperator.Between,
        ContentPolicyOperator.NotBetween,
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    private static readonly ContentPolicyOperator[] BoolOperators =
    [
        ContentPolicyOperator.Equals,
        ContentPolicyOperator.NotEquals,
        ContentPolicyOperator.Exists,
        ContentPolicyOperator.NotExists
    ];

    private static readonly ContentPolicyOperator[] CollectionOperators =
    [
        ContentPolicyOperator.Exists,
        ContentPolicyOperator.NotExists,
        ContentPolicyOperator.CountEquals,
        ContentPolicyOperator.CountNotEquals,
        ContentPolicyOperator.CountGreaterThan,
        ContentPolicyOperator.CountGreaterThanOrEqual,
        ContentPolicyOperator.CountLessThan,
        ContentPolicyOperator.CountLessThanOrEqual
    ];

    private static readonly ContentPolicyOperator[] NavigationOperators =
    [
        ContentPolicyOperator.Exists,
        ContentPolicyOperator.NotExists,
        ContentPolicyOperator.IsNull,
        ContentPolicyOperator.IsNotNull
    ];

    public static IReadOnlyList<ContentPolicyOperator> GetOperators(ContentPolicyPropertyKind kind, Type clrType)
    {
        if (kind == ContentPolicyPropertyKind.Collection)
            return CollectionOperators;

        if (kind == ContentPolicyPropertyKind.Navigation)
            return NavigationOperators;

        var underlying = Nullable.GetUnderlyingType(clrType) ?? clrType;

        if (underlying == typeof(string))
            return StringOperators;

        if (underlying == typeof(bool))
            return BoolOperators;

        if (underlying == typeof(int) || underlying == typeof(long) || underlying == typeof(short) || underlying == typeof(byte))
            return IntOperators;

        if (underlying == typeof(decimal) || underlying == typeof(double) || underlying == typeof(float))
            return ComparableOperators;

        if (underlying == typeof(DateTime) || underlying == typeof(DateTimeOffset) || underlying == typeof(DateOnly) || underlying == typeof(TimeOnly))
            return ComparableOperators;

        if (underlying.IsEnum)
            return IntOperators;

        if (underlying.IsValueType)
            return EqualityOperators;

        return NavigationOperators;
    }
}
