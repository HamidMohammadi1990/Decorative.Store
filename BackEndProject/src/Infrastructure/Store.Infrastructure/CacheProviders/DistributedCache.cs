using Edition.Application.Common.Caching.Enums;
using Edition.Application.Common.Caching.Abstractions;

namespace Store.Infrastructure.CacheProviders;

public class DistributedCache
    (IRedisCacheService redis, IDatabaseSelector databaseSelector) 
    : IDistributedCache
{
    public bool Exists(string key, CacheInstanceType cacheInstanceType)
        => redis.Exists(key, databaseSelector.Select(cacheInstanceType));

    public async Task<bool> ExistsAsync(string key, CacheInstanceType cacheInstanceType, CancellationToken token = default)
        => await redis.ExistsAsync(key, databaseSelector.Select(cacheInstanceType), token);

    public T? Get<T>(string key, CacheInstanceType cacheInstanceType)
      => redis.Get<T>(key, databaseSelector.Select(cacheInstanceType));

    public string? Get(string key, CacheInstanceType cacheInstanceType)
      => redis.Get(key, databaseSelector.Select(cacheInstanceType));

    public async Task<T?> GetAsync<T>(string key, CacheInstanceType cacheInstanceType, CancellationToken token = default)
      => await redis.GetAsync<T>(key, databaseSelector.Select(cacheInstanceType), token);

    public async Task<string?> GetAsync(string key, CacheInstanceType cacheInstanceType, CancellationToken token = default)
      => await redis.GetAsync<string>(key, databaseSelector.Select(cacheInstanceType), token);    

    public bool Remove(string key, CacheInstanceType cacheInstanceType)
      => redis.Remove(key, databaseSelector.Select(cacheInstanceType));

    public async Task<bool> RemoveAsync(string key, CacheInstanceType cacheInstanceType, CancellationToken token = default)
      => await redis.RemoveAsync(key, databaseSelector.Select(cacheInstanceType), token);

    public bool Set<T>(string key, T value, int duration, CacheInstanceType cacheInstanceType, bool extend = false) where T : class
      => redis.Set(key, value, duration, databaseSelector.Select(cacheInstanceType), extend);

    public bool Set<T>(string key, T value, TimeSpan duration, CacheInstanceType cacheInstanceType, bool extend = false) where T : class
      => redis.Set(key, value, duration, databaseSelector.Select(cacheInstanceType), extend);

    public bool Set<T>(string key, T value, DateTime duration, CacheInstanceType cacheInstanceType, bool extend = false) where T : class
      => redis.Set(key, value, duration, databaseSelector.Select(cacheInstanceType), extend);

    public async Task<bool> SetAsync<T>(string key, T value, int duration, CacheInstanceType cacheInstanceType, bool extend = false, CancellationToken token = default)
      => await redis.SetAsync(key, value, duration, databaseSelector.Select(cacheInstanceType), extend, token);

    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan duration, CacheInstanceType cacheInstanceType, bool extend = false, CancellationToken token = default)
      => await redis.SetAsync(key, value, duration, databaseSelector.Select(cacheInstanceType), extend, token);

    public async Task<bool> SetAsync<T>(string key, T value, DateTime duration, CacheInstanceType cacheInstanceType, bool extend = false, CancellationToken token = default)
      => await redis.SetAsync(key, value, duration, databaseSelector.Select(cacheInstanceType), extend, token);

    public TimeSpan? TimeToLive(string key, CacheInstanceType cacheInstanceType)
        => redis.TimeToLive(key, databaseSelector.Select(cacheInstanceType));

    public async Task<TimeSpan?> TimeToLiveAsync(string key, CacheInstanceType cacheInstanceType, CancellationToken token = default)
        => await redis.TimeToLiveAsync(key, databaseSelector.Select(cacheInstanceType), token);
}