using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum PermissionLevelType
{
    [Display(Name = "PermissionLevelType_Product", ResourceType = typeof(EnumResources))]
    Product = 1,

    [Display(Name = "PermissionLevelType_Tab", ResourceType = typeof(EnumResources))]
    Tab = 2,

    [Display(Name = "PermissionLevelType_Page", ResourceType = typeof(EnumResources))]
    Page = 3,

    [Display(Name = "PermissionLevelType_Action", ResourceType = typeof(EnumResources))]
    Action = 4
}
