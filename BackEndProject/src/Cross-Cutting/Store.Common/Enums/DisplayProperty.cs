using System.ComponentModel.DataAnnotations;
using Store.Common.Resources;

namespace Store.Common.Enums;

public enum DisplayProperty
{
    [Display(Name = "DisplayProperty_Description", ResourceType = typeof(EnumResources))]
    Description,

    [Display(Name = "DisplayProperty_GroupName", ResourceType = typeof(EnumResources))]
    GroupName,

    [Display(Name = "DisplayProperty_Name", ResourceType = typeof(EnumResources))]
    Name,

    [Display(Name = "DisplayProperty_Prompt", ResourceType = typeof(EnumResources))]
    Prompt,

    [Display(Name = "DisplayProperty_ShortName", ResourceType = typeof(EnumResources))]
    ShortName,

    [Display(Name = "DisplayProperty_Order", ResourceType = typeof(EnumResources))]
    Order
}
