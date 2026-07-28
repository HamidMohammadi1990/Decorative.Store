using Edition.Application.Common.Caching.Enums;

namespace Edition.Application.Configurations.ContentPolicies;

public class ContentPolicyCacheConfiguration : IContentPolicyCacheConfiguration
{
    public CacheInstanceType CacheInstance { get; set; } = CacheInstanceType.Permissions;
    public string GenerationKey { get; set; } = "ContentPolicy:Generation";
    public int UserContextTtlMinutes { get; set; } = 15;
    public int PoliciesTtlMinutes { get; set; } = 30;
    public int GenerationTtlDays { get; set; } = 3650;

    public TimeSpan UserContextTtl => TimeSpan.FromMinutes(UserContextTtlMinutes);
    public TimeSpan PoliciesTtl => TimeSpan.FromMinutes(PoliciesTtlMinutes);
    public TimeSpan GenerationTtl => TimeSpan.FromDays(GenerationTtlDays);
}
