using Newtonsoft.Json;
using StackExchange.Redis;
using Edition.Application.Common.Caching.Abstractions;
using Store.Common.Utilities;

namespace Store.Infrastructure.CacheProviders.Redis;

public class RedisCacheService
    (IConnectionMultiplexer connection)
    : IRedisCacheService
{
    public T? Get<T>(string key, int dbIndex = 0) => GetAsync<T>(key, dbIndex).Result;

    private static string? GetKey(string? key) => string.IsNullOrEmpty(key) ? null : key.Replace(" ", "").Trim();

    public string? Get(string key, int dbIndex = 0) => GetAsync<string>(key, dbIndex).Result;

    public async Task<T?> GetAsync<T>(string? key, int dbIndex = 0, CancellationToken token = default)
    {
        key = GetKey(key);
        if (key is null) return default;

        token.ThrowIfCancellationRequested();
        var db = connection.GetDatabase(dbIndex);
        if (typeof(T).IsPrimitive)
        {
            var cacheValue = await db.StringGetAsync(key);
            var decompressCacheValue = GZipUtility.DeCompress(cacheValue!);
            return GetValue<T>(decompressCacheValue);
        }
        var cacheBinary = await db.StringGetAsync(key);
        if (!cacheBinary.HasValue) return default;
        try
        {
            var decompresscacheBinary = GZipUtility.DeCompress(cacheBinary!);
            var result = JsonConvert.DeserializeObject<T>(decompresscacheBinary!);
            return result ?? default;
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public bool Remove(string? key, int dbIndex = 0)
    {
        key = GetKey(key);
        if (key is null) return false;

        var db = connection.GetDatabase(dbIndex);
        return db.KeyDelete(key);
    }

    public TimeSpan? TimeToLive(string key, int dbIndex = 0)
    {
        if (key is null) return default;

        var db = connection.GetDatabase(dbIndex);
        return db.KeyTimeToLive(key);
    }

    public async Task<TimeSpan?> TimeToLiveAsync(string key, int dbIndex = 0, CancellationToken token = default)
    {
        if (key is null) return default;

        token.ThrowIfCancellationRequested();
        var db = connection.GetDatabase(dbIndex);
        return await db.KeyTimeToLiveAsync(key);
    }

    public bool Exists(string key, int dbIndex = 0)
    {
        if (key is null) return default;

        var db = connection.GetDatabase(dbIndex);
        return db.KeyExists(key);
    }

    public async Task<bool> ExistsAsync(string key, int dbIndex = 0, CancellationToken token = default)
    {
        if (key is null) return default;

        token.ThrowIfCancellationRequested();
        var db = connection.GetDatabase(dbIndex);
        return await db.KeyExistsAsync(key);
    }

    public async Task<bool> RemoveAsync(string? key, int dbIndex = 0, CancellationToken token = default)
    {
        key = GetKey(key);
        if (key is null) return false;
        var db = connection.GetDatabase(dbIndex);
        return await db.KeyDeleteAsync(key);
    }

    public bool Set<T>(string key, T value, TimeSpan duration, int dbIndex = 0, bool extend = false) where T : class
    {
        return SetAsync(key, value, duration, dbIndex, extend).Result;
    }

    public bool Set<T>(string key, T value, int duration, int dbIndex = 0, bool extend = false) where T : class
    {
        var timeSpan = DateTime.UtcNow.AddSeconds(duration) - DateTime.UtcNow;
        return Set(key, value, timeSpan, dbIndex, extend);
    }

    public bool Set<T>(string key, T value, DateTime duration, int dbIndex = 0, bool extend = false) where T : class
    {
        var timeSpan = duration - DateTime.UtcNow;
        return Set(key, value, timeSpan, dbIndex, extend);
    }

    public async Task<bool> SetAsync<T>(string? key, T value, TimeSpan duration, int dbIndex = 0, bool extend = false, CancellationToken token = default)
    {
        key = GetKey(key);
        if (key is null) return false;

        token.ThrowIfCancellationRequested();

        var db = connection.GetDatabase(dbIndex);
        if (!extend && await db.KeyExistsAsync(key)) return false;
        var compressedValue =
            typeof(T).IsPrimitive
            ? GZipUtility.Compress(value.ToString()!)
            : GZipUtility.Compress(JsonConvert.SerializeObject(value));
        await db.StringSetAsync(key, compressedValue, duration);
        return true;
    }

    public async Task<bool> SetAsync<T>(string key, T value, DateTime duration, int dbIndex = 0, bool extend = false, CancellationToken token = default)
    {
        var timeSpan = duration - DateTime.UtcNow;
        return await SetAsync(key, value, timeSpan, dbIndex, extend, token);
    }

    public async Task<bool> SetAsync<T>(string key, T value, int duration, int dbIndex = 0, bool extend = false, CancellationToken token = default)
    {
        var timeSpan = DateTime.UtcNow.AddSeconds(duration) - DateTime.UtcNow;
        return await SetAsync(key, value, timeSpan, dbIndex, extend, token);
    }

    private static T GetValue<T>(string value) => (T)Convert.ChangeType(value, typeof(T));
}