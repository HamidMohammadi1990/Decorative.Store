using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Store.Domain.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyPreviewService
    (
        IContentPolicyRepository contentPolicyRepository,
        IContentPolicyCache contentPolicyCache,
        ContentPolicyScenarioEvaluator scenarioEvaluator,
        ContentPolicyRuleValidator ruleValidator,
        IContentPolicyMapperService mapper,
        IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyPreviewService
{
    public async Task<ContentPolicyPreviewResultDto?> PreviewAsync(
        PreviewContentPolicyRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!entityTypeRegistry.IsRegistered(request.EntityType))
            return null;

        var userContext = await contentPolicyCache.GetUserContextAsync(request.UserId, cancellationToken);
        if (userContext is null)
            return null;

        var bypass = userContext.Roles.Any(x => x.BypassContentPolicy);
        var requirePolicy = userContext.Roles.Any(x => x.RequireContentPolicy);
        var resolution = await contentPolicyCache.ResolvePoliciesAsync(
            request.EntityType,
            request.QueryAction,
            request.UserId,
            userContext.RoleIds,
            cancellationToken);

        var scenario = await scenarioEvaluator.EvaluateAsync(
            request.UserId,
            request.EntityType,
            resolution,
            userContext,
            bypass,
            requirePolicy,
            request.SampleSize,
            cancellationToken);

        return mapper.MapPreviewResult(request, scenario, userContext, bypass, requirePolicy);
    }

    public async Task<ContentPolicyMergeCompareResultDto?> CompareMergeAsync(
        CompareContentPolicyMergeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!entityTypeRegistry.IsRegistered(request.EntityType))
            return null;

        if (request.DraftUserPolicy?.Rules.Count > 0)
        {
            var errors = ruleValidator.ValidateRules(request.EntityType, request.DraftUserPolicy.Rules);
            if (errors.Count > 0)
                return null;
        }

        var userContext = await contentPolicyCache.GetUserContextAsync(request.UserId, cancellationToken);
        if (userContext is null)
            return null;

        var bypass = userContext.Roles.Any(x => x.BypassContentPolicy);
        var requirePolicy = userContext.Roles.Any(x => x.RequireContentPolicy);

        var policySets = await contentPolicyRepository.GetActivePolicySetsAsync(
            mapper.MapPolicySetsRequest(request, userContext.RoleIds),
            cancellationToken);

        var userPolicies = policySets.UserPolicies.ToList();
        if (request.DraftUserPolicy is { Rules.Count: > 0 })
            userPolicies.Add(mapper.MapDraftPolicy(request));

        var rolePolicies = policySets.RolePolicies;

        var currentResolution = ContentPolicyPolicyResolver.Resolve(userPolicies, rolePolicies);
        var roleOnlyResolution = ContentPolicyPolicyResolver.ResolveForMergeMode([], rolePolicies, ContentPolicyMergeMode.Additive);
        var additiveResolution = ContentPolicyPolicyResolver.ResolveForMergeMode(userPolicies, rolePolicies, ContentPolicyMergeMode.Additive);
        var replaceRoleResolution = ContentPolicyPolicyResolver.ResolveForMergeMode(userPolicies, rolePolicies, ContentPolicyMergeMode.ReplaceRole);

        var current = await scenarioEvaluator.EvaluateAsync(
            request.UserId, request.EntityType, currentResolution, userContext, bypass, requirePolicy, request.SampleSize, cancellationToken);
        var roleOnly = await scenarioEvaluator.EvaluateAsync(
            request.UserId, request.EntityType, roleOnlyResolution, userContext, bypass, requirePolicy, request.SampleSize, cancellationToken);
        var additive = await scenarioEvaluator.EvaluateAsync(
            request.UserId, request.EntityType, additiveResolution, userContext, bypass, requirePolicy, request.SampleSize, cancellationToken);
        var replaceRole = await scenarioEvaluator.EvaluateAsync(
            request.UserId, request.EntityType, replaceRoleResolution, userContext, bypass, requirePolicy, request.SampleSize, cancellationToken);

        return mapper.MapCompareResult(
            request,
            userContext,
            bypass,
            requirePolicy,
            currentResolution.EffectiveMergeMode,
            current,
            roleOnly,
            additive,
            replaceRole);
    }
}
