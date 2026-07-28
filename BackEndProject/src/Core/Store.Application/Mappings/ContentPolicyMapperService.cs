using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Features.ContentPolicies.Queries;
using Edition.Application.Features.ContentPolicyMetadata.Queries;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ContentPolicyMapperService : IContentPolicyMapperService
{
    public GetAllContentPolicyRequestDto Map(GetAllContentPolicyRequest model)
        => new()
        {
            Name = model.Name,
            RoleId = model.RoleId,
            UserId = model.UserId,
            IsActive = model.IsActive,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            Pagination = model.Pagination
        };

    public PagedResult<GetAllContentPolicyResponse> Map(PagedResult<ContentPolicy> model)
    {
        var items = model.Items.Select(MapListItem).ToList();
        return PagedResult<GetAllContentPolicyResponse>.Create(items, model);
    }

    public GetContentPolicyResponse Map(ContentPolicy model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Scope = model.Scope,
            MergeMode = model.MergeMode,
            RoleId = model.RoleId,
            UserId = model.UserId,
            Effect = model.Effect,
            IsActive = model.IsActive,
            Priority = model.Priority,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            Rules = [.. model.Rules
                .OrderBy(x => x.RuleGroup)
                .ThenBy(x => x.SortOrder)
                .Select(x => new GetContentPolicyRuleSummaryResponse
                {
                    Id = x.Id,
                    Value = x.Value,
                    Operator = x.Operator,
                    FieldPath = x.FieldPath,
                    ValueType = x.ValueType,
                    SortOrder = x.SortOrder,
                    RuleGroup = x.RuleGroup
                })],
            RecordEntityIds = [.. model.RecordAccesses.OrderBy(x => x.EntityId).Select(x => x.EntityId)]
        };

    public GetContentPolicyEntitySchemaRequestDto Map(GetContentPolicyEntitySchemaRequest model)
        => new()
        {
            EntityType = model.EntityType,
            ParentPath = model.ParentPath
        };

    public GetContentPolicyEntitySchemaResponse Map(
        GetContentPolicyEntitySchemaRequest model,
        IReadOnlyList<ContentPolicySchemaPropertyDto> properties)
        => new()
        {
            EntityType = model.EntityType,
            ParentPath = model.ParentPath,
            Properties = properties
        };

    public GetContentPolicyPropertyOperatorsRequestDto Map(GetContentPolicyPropertyOperatorsRequest model)
        => new()
        {
            EntityType = model.EntityType,
            FieldPath = model.FieldPath
        };

    public GetContentPolicyPropertyOperatorsResponse Map(
        GetContentPolicyPropertyOperatorsRequestDto request,
        IReadOnlyList<ContentPolicyOperator> operators)
        => new()
        {
            FieldPath = request.FieldPath,
            Operators = [.. operators.Select(x => new ContentPolicyEnumOptionDto(Convert.ToInt32(x), x.ToString()))]
        };

    public ValidateContentPolicyRulesRequestDto Map(ValidateContentPolicyRulesRequest model)
        => new()
        {
            EntityType = model.EntityType,
            Rules = [.. model.Rules
                .Select(x => new ContentPolicyRuleDto(
                    x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))]
        };

    public ValidateContentPolicyRulesResponse Map(ContentPolicyRuleValidationResultDto model)
        => new()
        {
            IsValid = model.IsValid,
            Errors = model.Errors
        };

    public PreviewContentPolicyRequestDto Map(PreviewContentPolicyRequest model)
        => new()
        {
            UserId = model.UserId,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            SampleSize = Math.Clamp(model.SampleSize, 1, 50)
        };

    public PreviewContentPolicyResponse Map(ContentPolicyPreviewResultDto model)
        => new()
        {
            UserId = model.UserId,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            AccessMode = model.AccessMode,
            EffectiveMergeMode = model.EffectiveMergeMode,
            BypassContentPolicy = model.BypassContentPolicy,
            RequireContentPolicy = model.RequireContentPolicy,
            Roles = model.Roles,
            CompanyIds = model.CompanyIds,
            AppliedPolicies = model.AppliedPolicies,
            ExcludedRolePolicies = model.ExcludedRolePolicies,
            TotalEntityCount = model.TotalEntityCount,
            AccessibleEntityCount = model.AccessibleEntityCount,
            SampleAccessibleIds = model.SampleAccessibleIds
        };

    public CompareContentPolicyMergeRequestDto Map(CompareContentPolicyMergeRequest model)
        => new()
        {
            UserId = model.UserId,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            SampleSize = Math.Clamp(model.SampleSize, 1, 50),
            DraftUserPolicy = model.DraftUserPolicy is null
                ? null
                : new CompareContentPolicyDraftPolicyDto
                {
                    Name = model.DraftUserPolicy.Name,
                    Effect = model.DraftUserPolicy.Effect,
                    MergeMode = model.DraftUserPolicy.MergeMode,
                    Priority = model.DraftUserPolicy.Priority,
                    Rules = [.. model.DraftUserPolicy.Rules
                        .Select(x => new ContentPolicyRuleDto(
                            x.FieldPath, x.Operator, x.ValueType, x.Value, x.SortOrder, x.RuleGroup))]
                }
        };

    public CompareContentPolicyMergeResponse Map(ContentPolicyMergeCompareResultDto model)
        => new()
        {
            UserId = model.UserId,
            EntityType = model.EntityType,
            QueryAction = model.QueryAction,
            BypassContentPolicy = model.BypassContentPolicy,
            RequireContentPolicy = model.RequireContentPolicy,
            IncludesDraftPolicy = model.IncludesDraftPolicy,
            Roles = model.Roles,
            CompanyIds = model.CompanyIds,
            Current = model.Current,
            RoleOnly = model.RoleOnly,
            Additive = model.Additive,
            ReplaceRole = model.ReplaceRole,
            Diff = model.Diff
        };

    public GetContentPolicyPolicySetsRequestDto MapPolicySetsRequest(
        CompareContentPolicyMergeRequestDto request,
        IReadOnlyList<int> roleIds)
        => new()
        {
            EntityType = request.EntityType,
            QueryAction = request.QueryAction,
            UserId = request.UserId,
            RoleIds = roleIds
        };

    public ContentPolicyWithRulesDto MapDraftPolicy(CompareContentPolicyMergeRequestDto request)
    {
        var draft = request.DraftUserPolicy!;
        return new ContentPolicyWithRulesDto(
            Id: 0,
            RoleId: null,
            UserId: request.UserId,
            MergeMode: draft.MergeMode,
            EntityType: request.EntityType,
            QueryAction: request.QueryAction,
            Name: draft.Name,
            Effect: draft.Effect,
            Priority: draft.Priority,
            Rules: draft.Rules,
            RecordEntityIds: []);
    }

    public IReadOnlyList<ContentPolicyPreviewPolicyDto> Map(IReadOnlyList<ContentPolicyWithRulesDto> policies)
        => [.. policies
            .Select(x => new ContentPolicyPreviewPolicyDto(
                x.Id,
                x.UserId.HasValue ? ContentPolicyScope.User : ContentPolicyScope.Role,
                x.MergeMode,
                x.RoleId,
                x.UserId,
                x.Name,
                x.Effect,
                x.Priority,
                x.QueryAction,
                x.Rules,
                x.RecordEntityIds))];

    public ContentPolicyPreviewResultDto MapPreviewResult(
        PreviewContentPolicyRequestDto request,
        ContentPolicyScenarioPreviewDto scenario,
        CachedUserContentPolicyContext userContext,
        bool bypassContentPolicy,
        bool requireContentPolicy)
        => new(
            request.UserId,
            request.EntityType,
            request.QueryAction,
            scenario.AccessMode,
            scenario.EffectiveMergeMode,
            bypassContentPolicy,
            requireContentPolicy,
            userContext.Roles,
            userContext.CompanyIds,
            scenario.AppliedPolicies,
            scenario.ExcludedRolePolicies,
            scenario.TotalEntityCount,
            scenario.AccessibleEntityCount,
            scenario.SampleAccessibleIds);

    public ContentPolicyMergeCompareResultDto MapCompareResult(
        CompareContentPolicyMergeRequestDto request,
        CachedUserContentPolicyContext userContext,
        bool bypassContentPolicy,
        bool requireContentPolicy,
        ContentPolicyMergeMode currentMergeMode,
        ContentPolicyScenarioPreviewDto current,
        ContentPolicyScenarioPreviewDto roleOnly,
        ContentPolicyScenarioPreviewDto additive,
        ContentPolicyScenarioPreviewDto replaceRole)
    {
        var currentScenario = ToCompareScenario(currentMergeMode, current);
        var roleOnlyScenario = ToCompareScenario(ContentPolicyMergeMode.Additive, roleOnly);
        var additiveScenario = ToCompareScenario(ContentPolicyMergeMode.Additive, additive);
        var replaceRoleScenario = ToCompareScenario(ContentPolicyMergeMode.ReplaceRole, replaceRole);

        return new ContentPolicyMergeCompareResultDto(
            request.UserId,
            request.EntityType,
            request.QueryAction,
            bypassContentPolicy,
            requireContentPolicy,
            request.DraftUserPolicy is { Rules.Count: > 0 },
            userContext.Roles,
            userContext.CompanyIds,
            currentScenario,
            roleOnlyScenario,
            additiveScenario,
            replaceRoleScenario,
            BuildDiff(currentScenario, roleOnlyScenario, additiveScenario, replaceRoleScenario));
    }

    public IReadOnlyList<ContentPolicyEnumOptionDto> MapEnumOptions<TEnum>()
        where TEnum : struct, Enum
        => [.. Enum.GetValues<TEnum>().Select(x => new ContentPolicyEnumOptionDto(Convert.ToInt32(x), x.ToString()))];

    private static GetAllContentPolicyResponse MapListItem(ContentPolicy x)
        => new()
        {
            Id = x.Id,
            Name = x.Name,
            Scope = x.Scope,
            MergeMode = x.MergeMode,
            RoleId = x.RoleId,
            UserId = x.UserId,
            Effect = x.Effect,
            IsActive = x.IsActive,
            Priority = x.Priority,
            EntityType = x.EntityType,
            QueryAction = x.QueryAction,
            RuleCount = x.Rules.Count
        };

    private static ContentPolicyMergeCompareScenarioDto ToCompareScenario(
        ContentPolicyMergeMode simulatedMergeMode,
        ContentPolicyScenarioPreviewDto scenario)
        => new(
            simulatedMergeMode,
            scenario.AccessMode,
            scenario.AppliedPolicies.Count,
            scenario.ExcludedRolePolicies.Count,
            scenario.AppliedPolicies,
            scenario.ExcludedRolePolicies,
            scenario.TotalEntityCount,
            scenario.AccessibleEntityCount,
            scenario.SampleAccessibleIds);

    private static ContentPolicyMergeCompareDiffDto BuildDiff(
        ContentPolicyMergeCompareScenarioDto current,
        ContentPolicyMergeCompareScenarioDto roleOnly,
        ContentPolicyMergeCompareScenarioDto additive,
        ContentPolicyMergeCompareScenarioDto replaceRole)
        => new(
            replaceRole.AccessibleEntityCount - additive.AccessibleEntityCount,
            replaceRole.AccessibleEntityCount - roleOnly.AccessibleEntityCount,
            additive.AccessibleEntityCount - roleOnly.AccessibleEntityCount,
            current.AccessibleEntityCount - additive.AccessibleEntityCount,
            [.. additive.SampleAccessibleIds.Except(replaceRole.SampleAccessibleIds)],
            [.. replaceRole.SampleAccessibleIds.Except(additive.SampleAccessibleIds)],
            [.. current.SampleAccessibleIds.Except(additive.SampleAccessibleIds)]);
}
