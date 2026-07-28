using Store.Domain.Enums;

namespace Store.Domain.Dtos.ContentPolicies;

public record ContentPolicyRuleDto(
    string FieldPath,
    ContentPolicyOperator Operator,
    ContentPolicyValueType ValueType,
    string Value,
    int SortOrder,
    ContentPolicyRuleGroup RuleGroup);

public record ContentPolicyWithRulesDto(
    int Id,
    int? RoleId,
    int? UserId,
    ContentPolicyMergeMode MergeMode,
    string EntityType,
    ContentPolicyQueryAction QueryAction,
    string Name,
    ContentPolicyEffect Effect,
    int Priority,
    IReadOnlyList<ContentPolicyRuleDto> Rules,
    IReadOnlyList<int> RecordEntityIds);

public record ContentPolicyResolutionResult(
    ContentPolicyMergeMode EffectiveMergeMode,
    IReadOnlyList<ContentPolicyWithRulesDto> AppliedPolicies,
    IReadOnlyList<ContentPolicyWithRulesDto> ExcludedRolePolicies);

public record ContentPolicyEntityPreviewData(
    int TotalEntityCount,
    int AccessibleEntityCount,
    IReadOnlyList<int> SampleAccessibleIds);

public record UserRolePolicyDto(
    int Id,
    string Title,
    bool BypassContentPolicy,
    bool RequireContentPolicy);

public sealed record UserContentPolicyContextData(
    IReadOnlyList<UserRolePolicyDto> Roles,
    IReadOnlyList<int> CompanyIds);