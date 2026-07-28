namespace Store.Domain.Enums;

/// <summary>
/// Controls how a user-scoped policy combines with role-scoped policies.
/// Ignored for role-scoped policies.
/// </summary>
public enum ContentPolicyMergeMode
{
    /// <summary>User policies are merged with matching role policies.</summary>
    Additive = 0,

    /// <summary>When any user policy uses this mode, role policies are excluded.</summary>
    ReplaceRole = 1
}
