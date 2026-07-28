using Edition.Application.Common.Caching.Enums;

namespace Edition.Application.Configurations.Localization;

public sealed class LanguageCacheConfiguration : ILanguageCacheConfiguration
{
    public string DataKey { get; init; } = "edition:languages:data";
    public string VersionKey { get; init; } = "edition:languages:version";
    public CacheInstanceType CacheInstance { get; init; } = CacheInstanceType.AppSettings;
    public TimeSpan CacheTtl { get; init; } = TimeSpan.FromDays(30);
}
