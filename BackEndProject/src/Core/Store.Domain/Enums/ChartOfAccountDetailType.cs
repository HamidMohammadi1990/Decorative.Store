using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ChartOfAccountDetailType
{
    [Display(Name = "ChartOfAccountDetailType_None", ResourceType = typeof(EnumResources))]
    None = 0,

    [Display(Name = "ChartOfAccountDetailType_Shakhs", ResourceType = typeof(EnumResources))]
    Shakhs = 1,

    [Display(Name = "ChartOfAccountDetailType_Bank", ResourceType = typeof(EnumResources))]
    Bank = 2,

    [Display(Name = "ChartOfAccountDetailType_Sandogh", ResourceType = typeof(EnumResources))]
    Sandogh = 3,

    [Display(Name = "ChartOfAccountDetailType_TankhahGardan", ResourceType = typeof(EnumResources))]
    TankhahGardan = 4,

    [Display(Name = "ChartOfAccountDetailType_Kala", ResourceType = typeof(EnumResources))]
    Kala = 5,

    [Display(Name = "ChartOfAccountDetailType_Hesab", ResourceType = typeof(EnumResources))]
    Hesab = 6
    // تفصیل هامون که حساب و صندوقق  و بانک اینا بود
}
