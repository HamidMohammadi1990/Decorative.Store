using Store.Domain.Enums;

namespace Store.Domain.ContentPolicies;

public static class ContentPolicyQueryActionExtensions
{
    public static bool Matches(ContentPolicyQueryAction policyAction, ContentPolicyQueryAction requestAction)
        => policyAction == ContentPolicyQueryAction.All || policyAction == requestAction;
}
