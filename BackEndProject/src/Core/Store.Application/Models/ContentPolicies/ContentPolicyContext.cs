namespace Edition.Application.Models.ContentPolicies;

public sealed record ContentPolicyContext(
    int UserId,
    IReadOnlyList<int> RoleIds);
