using Edition.Application.Common.Caching.Enums;

namespace Edition.Application.Configurations.Localization;

public interface ILanguageCacheConfiguration
{
    string DataKey { get; }
    string VersionKey { get; }
    CacheInstanceType CacheInstance { get; }
    TimeSpan CacheTtl { get; }
}
