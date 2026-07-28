using System.Linq.Expressions;

namespace Store.Domain.ContentPolicies;

public static class ContentPolicyFilterExpressions
{
    public static bool IsDenyAll(LambdaExpression? filter)
        => filter?.Body is ConstantExpression { Value: false };
}
