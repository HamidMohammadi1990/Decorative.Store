using RedLockNet.SERedis;
using Castle.DynamicProxy;
using Castle.Core.Internal;
using Microsoft.Extensions.Logging;
using Edition.Application.Common.Caching.Attributes;
using Edition.Application.Common.Caching.Abstractions;
using Store.Infrastructure.Extensions;

namespace Store.Infrastructure.Interceptors;

public sealed class CacheInterceptor
    (IDistributedCache cache, ILogger<CacheInterceptor> logger, RedLockFactory redLockFactory)
    : IAsyncInterceptor
{
    public void InterceptSynchronous(IInvocation invocation)
    {
        invocation.Proceed();
    }

    public void InterceptAsynchronous(IInvocation invocation)
    {
        invocation.ReturnValue = InterceptAsync(invocation);
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = InterceptAsync<TResult>(invocation);
    }

    private async Task InterceptAsync(IInvocation invocation)
    {
        var attr = invocation.MethodInvocationTarget.GetAttribute<CacheAttribute>();
        if (attr is null || !attr.Idempotent)
        {
            invocation.Proceed();
            await (Task)invocation.ReturnValue!;
            return;
        }

        var key = $"idempotent:{invocation.GetMethodKey()}";
        var token = invocation.GetCancellationToken();

        // already executed?
        var executed = await cache.GetAsync<bool>(key, attr.CacheInstance, token);
        if (executed)
        {
            logger.LogDebug("Idempotent call skipped: {Key}", key);
            return;
        }

        await using var redLock =
            await redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(10));

        if (!redLock.IsAcquired)
        {
            logger.LogWarning("Idempotent lock not acquired: {Key}", key);
            return;
        }

        invocation.Proceed();
        await (Task)invocation.ReturnValue!;

        await cache.SetAsync(
            key,
            true,
            attr.Duration,
            attr.CacheInstance,
            extend: false,
            token);
    }

    private async Task<TResult?> InterceptAsync<TResult>(IInvocation invocation)
    {
        var cacheAttr = invocation.MethodInvocationTarget.GetAttribute<CacheAttribute>();
        if (cacheAttr is null)
        {
            invocation.Proceed();
            return await (Task<TResult>)invocation.ReturnValue!;
        }

        var key = invocation.GetMethodKey();
        var token = invocation.GetCancellationToken();

        var cached = await cache.GetAsync<TResult>(key, cacheAttr.CacheInstance, token);
        if (cached is not null)
            return cached;

        await using var redLock = await redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(10));
        if (!redLock.IsAcquired)
            return default;

        var result = await invocation.ProceedAsync<TResult>();
        if (result is not null)
        {
            await cache.SetAsync(
                key,
                result,
                cacheAttr.Duration,
                cacheAttr.CacheInstance,
                cacheAttr.Extend,
                token);
        }

        invocation.ReturnValue = Task.FromResult(result);
        return result;
    }
}