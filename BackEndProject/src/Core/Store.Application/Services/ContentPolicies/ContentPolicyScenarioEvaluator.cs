using System.Linq.Expressions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyScenarioEvaluator
    (
        ContentPolicyExpressionBuilder expressionBuilder,
        IContentEntityTypeRegistry entityTypeRegistry,
        IContentPolicyEntityPreviewQuery entityPreviewQuery,
        IContentPolicyMapperService mapper)
{
    public async Task<ContentPolicyScenarioPreviewDto> EvaluateAsync(
        int userId,
        string entityType,
        ContentPolicyResolutionResult resolution,
        CachedUserContentPolicyContext userContext,
        bool bypassContentPolicy,
        bool requireContentPolicy,
        int sampleSize,
        CancellationToken cancellationToken = default)
    {
        var filter = bypassContentPolicy
            ? null
            : CompileFilter(userId, entityType, resolution.AppliedPolicies, userContext, requireContentPolicy);

        var accessMode = ContentPolicyFilter.ResolveAccessMode(
            bypassContentPolicy,
            requireContentPolicy,
            filter,
            resolution.AppliedPolicies.Count);

        var previewData = await LoadPreviewDataAsync(
            entityType,
            filter,
            accessMode,
            sampleSize,
            cancellationToken);

        return new ContentPolicyScenarioPreviewDto(
            resolution.EffectiveMergeMode,
            accessMode,
            mapper.Map(resolution.AppliedPolicies),
            mapper.Map(resolution.ExcludedRolePolicies),
            previewData.TotalEntityCount,
            previewData.AccessibleEntityCount,
            previewData.SampleAccessibleIds);
    }

    private LambdaExpression? CompileFilter(
        int userId,
        string entityType,
        IReadOnlyList<ContentPolicyWithRulesDto> policies,
        CachedUserContentPolicyContext userContext,
        bool requireContentPolicy)
    {
        if (policies.Count == 0)
            return requireContentPolicy ? BuildDenyAll(entityType) : null;

        var policyContext = new ContentPolicyContext(
            userId,
            userContext.CompanyIds,
            userContext.RoleIds);

        var filter = ContentPolicyExpressionBuildInvoker.Build(
            expressionBuilder,
            entityTypeRegistry,
            entityType,
            policies,
            policyContext);

        if (filter is null && requireContentPolicy)
            return BuildDenyAll(entityType);

        return filter;
    }

    private async Task<ContentPolicyEntityPreviewData> LoadPreviewDataAsync(
        string entityType,
        LambdaExpression? filter,
        ContentPolicyAccessMode accessMode,
        int sampleSize,
        CancellationToken cancellationToken)
    {
        if (accessMode == ContentPolicyAccessMode.DenyAll)
        {
            var totalOnly = await entityPreviewQuery.PreviewAsync(entityType, null, sampleSize, cancellationToken);
            return new ContentPolicyEntityPreviewData(totalOnly.TotalEntityCount, 0, []);
        }

        return await entityPreviewQuery.PreviewAsync(entityType, filter, sampleSize, cancellationToken);
    }

    private LambdaExpression BuildDenyAll(string entityType)
    {
        var entityClrType = entityTypeRegistry.GetClrType(entityType);
        var parameter = Expression.Parameter(entityClrType, "entity");
        return Expression.Lambda(Expression.Constant(false), parameter);
    }
}
