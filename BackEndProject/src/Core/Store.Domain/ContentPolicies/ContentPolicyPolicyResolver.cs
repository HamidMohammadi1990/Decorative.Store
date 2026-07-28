using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Enums;

namespace Store.Domain.ContentPolicies;

public static class ContentPolicyPolicyResolver
{
    public static ContentPolicyResolutionResult Resolve(
        IReadOnlyList<ContentPolicyWithRulesDto> userPolicies,
        IReadOnlyList<ContentPolicyWithRulesDto> rolePolicies)
        => ResolveForMergeMode(userPolicies, rolePolicies, null);

    public static ContentPolicyResolutionResult ResolveForMergeMode(
        IReadOnlyList<ContentPolicyWithRulesDto> userPolicies,
        IReadOnlyList<ContentPolicyWithRulesDto> rolePolicies,
        ContentPolicyMergeMode? forcedMergeMode)
    {
        var replaceRole = forcedMergeMode switch
        {
            ContentPolicyMergeMode.ReplaceRole => userPolicies.Count > 0,
            ContentPolicyMergeMode.Additive => false,
            null => userPolicies.Count > 0
                && userPolicies.Any(x => x.MergeMode == ContentPolicyMergeMode.ReplaceRole),
            _ => false
        };

        if (replaceRole)
        {
            return new ContentPolicyResolutionResult(
                ContentPolicyMergeMode.ReplaceRole,
                Order(userPolicies),
                Order(rolePolicies));
        }

        return new ContentPolicyResolutionResult(
            ContentPolicyMergeMode.Additive,
            Order([.. userPolicies, .. rolePolicies]),
            []);
    }

    private static IReadOnlyList<ContentPolicyWithRulesDto> Order(IEnumerable<ContentPolicyWithRulesDto> policies)
        => policies
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .ToList();
}
