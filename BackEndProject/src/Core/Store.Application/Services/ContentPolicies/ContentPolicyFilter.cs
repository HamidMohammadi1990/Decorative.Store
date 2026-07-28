using System.Text;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Edition.Application.Contracts;
using Edition.Application.Contracts.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.ContentPolicies;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyFilter
    (
        ICurrentUserContext currentUser,
        IContentPolicyCache contentPolicyCache,
        ContentPolicyExpressionBuilder expressionBuilder,
        ContentPolicyCompiledFilterCache compiledFilterCache,
        IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyFilter
{
    public Task<LambdaExpression?> BuildFilterAsync(
        string entityType,
        ContentPolicyQueryAction queryAction,
        CancellationToken cancellationToken = default)
    {
        var userId = currentUser.UserId;
        if (userId <= 0)
            return Task.FromResult<LambdaExpression?>(null);

        return BuildFilterForUserAsync(userId, entityType, queryAction, cancellationToken);
    }

    public async Task<LambdaExpression?> BuildFilterForUserAsync(
        int userId,
        string entityType,
        ContentPolicyQueryAction queryAction,
        CancellationToken cancellationToken = default)
    {
        if (!entityTypeRegistry.IsRegistered(entityType))
            return null;

        var userContext = await contentPolicyCache.GetUserContextAsync(userId, cancellationToken);
        if (userContext is null)
            return null;

        if (userContext.Roles.Any(x => x.BypassContentPolicy))
            return null;

        var requiresPolicy = userContext.Roles.Any(x => x.RequireContentPolicy);
        var roleIds = userContext.RoleIds;
        var resolution = await contentPolicyCache.ResolvePoliciesAsync(
            entityType,
            queryAction,
            userId,
            roleIds,
            cancellationToken);

        if (resolution.AppliedPolicies.Count == 0)
            return requiresPolicy ? BuildDenyAll(entityType) : null;

        var generation = await contentPolicyCache.GetGenerationAsync(cancellationToken);
        var cacheKey = BuildCompiledFilterCacheKey(
            generation,
            entityType,
            queryAction,
            userId,
            roleIds,
            userContext.CompanyIds,
            resolution);
        var policyContext = new Models.ContentPolicies.ContentPolicyContext(
            userId,
            userContext.CompanyIds,
            userContext.RoleIds);

        var filter = compiledFilterCache.GetOrAdd(
            cacheKey,
            () => ContentPolicyExpressionBuildInvoker.Build(
                expressionBuilder,
                entityTypeRegistry,
                entityType,
                resolution.AppliedPolicies,
                policyContext));

        if (filter is null && requiresPolicy)
            return BuildDenyAll(entityType);

        return filter;
    }

    internal static ContentPolicyAccessMode ResolveAccessMode(
        bool bypassContentPolicy,
        bool requireContentPolicy,
        LambdaExpression? filter,
        int appliedPolicyCount)
    {
        if (bypassContentPolicy)
            return ContentPolicyAccessMode.Unrestricted;

        if (filter is null)
        {
            if (requireContentPolicy && appliedPolicyCount == 0)
                return ContentPolicyAccessMode.DenyAll;

            return ContentPolicyAccessMode.Unrestricted;
        }

        return ContentPolicyFilterExpressions.IsDenyAll(filter)
            ? ContentPolicyAccessMode.DenyAll
            : ContentPolicyAccessMode.Filtered;
    }

    internal static bool IsDenyAll(LambdaExpression filter)
        => ContentPolicyFilterExpressions.IsDenyAll(filter);

    private LambdaExpression BuildDenyAll(string entityType)
    {
        var entityClrType = entityTypeRegistry.GetClrType(entityType);
        var parameter = Expression.Parameter(entityClrType, "entity");
        var body = Expression.Constant(false);
        return Expression.Lambda(body, parameter);
    }

    private static string BuildCompiledFilterCacheKey(
        long generation,
        string entityType,
        ContentPolicyQueryAction queryAction,
        int userId,
        IReadOnlyList<int> roleIds,
        IReadOnlyList<int> companyIds,
        ContentPolicyResolutionResult resolution)
    {
        var builder = new StringBuilder(128);
        builder.Append(generation).Append(':').Append(entityType).Append(':').Append((int)queryAction).Append(':').Append(userId).Append(':');
        builder.Append(string.Join(',', roleIds.OrderBy(x => x))).Append(':');
        builder.Append(string.Join(',', companyIds.OrderBy(x => x))).Append(':');
        builder.Append((int)resolution.EffectiveMergeMode).Append(':');
        builder.Append(string.Join(',', resolution.AppliedPolicies.OrderBy(x => x.Priority).ThenBy(x => x.Id).Select(x => x.Id)));
        builder.Append(':');
        builder.Append(string.Join(',', resolution.ExcludedRolePolicies.OrderBy(x => x.Priority).ThenBy(x => x.Id).Select(x => x.Id)));

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()));
        return Convert.ToHexString(hash);
    }
}
