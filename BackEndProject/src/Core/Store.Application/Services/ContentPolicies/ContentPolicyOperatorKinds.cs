using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

internal static class ContentPolicyOperatorKinds
{
    internal static bool IsCountOperator(ContentPolicyOperator @operator)
        => @operator is ContentPolicyOperator.CountEquals
            or ContentPolicyOperator.CountNotEquals
            or ContentPolicyOperator.CountGreaterThan
            or ContentPolicyOperator.CountGreaterThanOrEqual
            or ContentPolicyOperator.CountLessThan
            or ContentPolicyOperator.CountLessThanOrEqual;

    internal static bool IsBetweenOperator(ContentPolicyOperator @operator)
        => @operator is ContentPolicyOperator.Between or ContentPolicyOperator.NotBetween;
}
