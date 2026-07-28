using System.ComponentModel.DataAnnotations;
using Edition.Application.Resources;

namespace Edition.Application.Common.Caching.Enums;

public enum CacheInstanceType
{
    [Display(Name = "CacheInstanceType_Default", ResourceType = typeof(EnumResources))]
    Default,

    [Display(Name = "CacheInstanceType_Permissions", ResourceType = typeof(EnumResources))]
    Permissions,

    [Display(Name = "CacheInstanceType_UserTokens", ResourceType = typeof(EnumResources))]
    UserTokens,

    [Display(Name = "CacheInstanceType_AppSettings", ResourceType = typeof(EnumResources))]
    AppSettings
}
