using Store.Domain.Enums;

namespace Store.Domain.Dtos.ContentPolicies;

public record ContentPolicyPreviewPolicyDto(
    int Id,
    ContentPolicyScope Scope,
    ContentPolicyMergeMode MergeMode,
    int? RoleId,
    int? UserId,
    string Name,
    ContentPolicyEffect Effect,
    int Priority,
    ContentPolicyQueryAction QueryAction,
    IReadOnlyList<ContentPolicyRuleDto> Rules,
    IReadOnlyList<int> RecordEntityIds);

public record ContentPolicyActivePolicySets(
    IReadOnlyList<ContentPolicyWithRulesDto> UserPolicies,
    IReadOnlyList<ContentPolicyWithRulesDto> RolePolicies);

public record ContentPolicyScenarioPreviewDto(
    ContentPolicyMergeMode EffectiveMergeMode,
    ContentPolicyAccessMode AccessMode,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> AppliedPolicies,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> ExcludedRolePolicies,
    int TotalEntityCount,
    int AccessibleEntityCount,
    IReadOnlyList<int> SampleAccessibleIds);

public record ContentPolicyMergeCompareScenarioDto(
    ContentPolicyMergeMode SimulatedMergeMode,
    ContentPolicyAccessMode AccessMode,
    int AppliedPolicyCount,
    int ExcludedRolePolicyCount,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> AppliedPolicies,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> ExcludedRolePolicies,
    int TotalEntityCount,
    int AccessibleEntityCount,
    IReadOnlyList<int> SampleAccessibleIds);

public record ContentPolicyMergeCompareDiffDto(
    int AccessibleCountDeltaReplaceRoleVsAdditive,
    int AccessibleCountDeltaReplaceRoleVsRoleOnly,
    int AccessibleCountDeltaAdditiveVsRoleOnly,
    int AccessibleCountDeltaCurrentVsAdditive,
    IReadOnlyList<int> SampleIdsOnlyInAdditive,
    IReadOnlyList<int> SampleIdsOnlyInReplaceRole,
    IReadOnlyList<int> SampleIdsOnlyInCurrent);

public record ContentPolicyMergeCompareResultDto(
    int UserId,
    string EntityType,
    ContentPolicyQueryAction QueryAction,
    bool BypassContentPolicy,
    bool RequireContentPolicy,
    bool IncludesDraftPolicy,
    IReadOnlyList<UserRolePolicyDto> Roles,
    ContentPolicyMergeCompareScenarioDto Current,
    ContentPolicyMergeCompareScenarioDto RoleOnly,
    ContentPolicyMergeCompareScenarioDto Additive,
    ContentPolicyMergeCompareScenarioDto ReplaceRole,
    ContentPolicyMergeCompareDiffDto Diff);

public record ContentPolicyPreviewResultDto(
    int UserId,
    string EntityType,
    ContentPolicyQueryAction QueryAction,
    ContentPolicyAccessMode AccessMode,
    ContentPolicyMergeMode EffectiveMergeMode,
    bool BypassContentPolicy,
    bool RequireContentPolicy,
    IReadOnlyList<UserRolePolicyDto> Roles,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> AppliedPolicies,
    IReadOnlyList<ContentPolicyPreviewPolicyDto> ExcludedRolePolicies,
    int TotalEntityCount,
    int AccessibleEntityCount,
    IReadOnlyList<int> SampleAccessibleIds);
