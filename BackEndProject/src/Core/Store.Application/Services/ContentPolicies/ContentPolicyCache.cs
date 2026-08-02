using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Caching.Abstractions;
using Edition.Application.Configurations.ContentPolicies;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Repositories;
using Store.Domain.Enums;

namespace Edition.Application.Services.ContentPolicies;

public sealed class ContentPolicyCache
    (
        IDistributedCache cache,
        IContentPolicyRepository contentPolicyRepository,
        ContentPolicyCompiledFilterCache compiledFilterCache,
        IContentPolicyCacheConfiguration configuration)
    : IContentPolicyCache
{
    public async Task<CachedUserContentPolicyContext?> GetUserContextAsync(int userId, CancellationToken cancellationToken = default)
    {
        var cached = await cache.GetAsync<CachedUserContentPolicyContext>(
            GetUserContextKey(userId),
            configuration.CacheInstance,
            cancellationToken);
        if (cached is not null)
            return cached;

        var contextData = await contentPolicyRepository.GetUserContentPolicyContextAsync(userId, cancellationToken);
        if (contextData is null)
            return null;

        var context = new CachedUserContentPolicyContext(
            contextData.Roles,
            [.. contextData.Roles.Select(x => x.Id)]);

        await SetUserContextAsync(userId, context, cancellationToken);
        return context;
    }

    public Task SetUserContextAsync(int userId, CachedUserContentPolicyContext context, CancellationToken cancellationToken = default)
        => cache.SetAsync(
            GetUserContextKey(userId),
            context,
            configuration.UserContextTtl,
            configuration.CacheInstance,
            token: cancellationToken);

    public async Task<ContentPolicyResolutionResult> ResolvePoliciesAsync(
        string entityType,
        ContentPolicyQueryAction queryAction,
        int userId,
        IReadOnlyList<int> roleIds,
        CancellationToken cancellationToken = default)
    {
        var generation = await GetGenerationAsync(cancellationToken);
        var cacheKey = GetPoliciesKey(generation, entityType, queryAction, userId, roleIds);
        var cached = await cache.GetAsync<CachedContentPolicyResolution>(cacheKey, configuration.CacheInstance, cancellationToken);
        if (cached is not null)
            return cached.Resolution;

        var resolution = await contentPolicyRepository.ResolveActivePoliciesAsync(
            entityType,
            queryAction,
            userId,
            roleIds,
            cancellationToken);
        await cache.SetAsync(
            cacheKey,
            new CachedContentPolicyResolution(resolution, DateTime.UtcNow),
            configuration.PoliciesTtl,
            configuration.CacheInstance,
            token: cancellationToken);

        return resolution;
    }

    public Task InvalidateUserAsync(int userId, CancellationToken cancellationToken = default)
        => cache.RemoveAsync(GetUserContextKey(userId), configuration.CacheInstance, cancellationToken);

    public async Task InvalidateAllAsync(CancellationToken cancellationToken = default)
    {
        var generation = await GetGenerationAsync(cancellationToken);
        await cache.SetAsync(
            configuration.GenerationKey,
            generation + 1,
            configuration.GenerationTtl,
            configuration.CacheInstance,
            token: cancellationToken);
        compiledFilterCache.Clear();
    }

    public async Task<long> GetGenerationAsync(CancellationToken cancellationToken = default)
    {
        var generation = await cache.GetAsync<long>(configuration.GenerationKey, configuration.CacheInstance, cancellationToken);
        return generation;
    }

    private static string GetUserContextKey(int userId) => $"ContentPolicy:UserContext:{userId}";

    private static string GetPoliciesKey(
        long generation,
        string entityType,
        ContentPolicyQueryAction queryAction,
        int userId,
        IReadOnlyList<int> roleIds)
    {
        var roleKey = string.Join(',', roleIds.OrderBy(x => x));
        return $"ContentPolicy:Policies:{generation}:{entityType}:{(int)queryAction}:{userId}:{roleKey}";
    }
}
