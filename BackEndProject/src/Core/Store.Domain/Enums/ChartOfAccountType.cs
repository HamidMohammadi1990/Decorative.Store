using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum ChartOfAccountType
{
    [Display(Name = "ChartOfAccountType_None", ResourceType = typeof(EnumResources))]
    None = 0,

    [Display(Name = "ChartOfAccountType_DaraeiHa", ResourceType = typeof(EnumResources))]
    DaraeiHa = 1,

    [Display(Name = "ChartOfAccountType_BedehiHa", ResourceType = typeof(EnumResources))]
    BedehiHa = 2,

    [Display(Name = "ChartOfAccountType_HoghughSahebanSaham", ResourceType = typeof(EnumResources))]
    HoghughSahebanSaham = 3,

    [Display(Name = "ChartOfAccountType_Kharid", ResourceType = typeof(EnumResources))]
    Kharid = 4,

    [Display(Name = "ChartOfAccountType_Forush", ResourceType = typeof(EnumResources))]
    Forush = 5,

    [Display(Name = "ChartOfAccountType_Daramad", ResourceType = typeof(EnumResources))]
    Daramad = 6,

    [Display(Name = "ChartOfAccountType_Hazineha", ResourceType = typeof(EnumResources))]
    Hazineha = 7,

    [Display(Name = "ChartOfAccountType_SAYERHesabHa", ResourceType = typeof(EnumResources))]
    SAYERHesabHa = 8,

    [Display(Name = "ChartOfAccountType_BahayeTamamShodeyeKalayeForukhtehShodeh", ResourceType = typeof(EnumResources))]
    BahayeTamamShodeyeKalayeForukhtehShodeh = 9,
    // داریی  و هزینه و اون اصلی ها
}
