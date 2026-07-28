using Edition.Application.Common.Caching.Enums;

namespace Edition.Application.Configurations.ContentPolicies;

public interface IContentPolicyCacheConfiguration
{
    CacheInstanceType CacheInstance { get; }
    string GenerationKey { get; }
    TimeSpan UserContextTtl { get; }
    TimeSpan PoliciesTtl { get; }
    TimeSpan GenerationTtl { get; }
}
