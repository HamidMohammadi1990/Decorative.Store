using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum CurrencyCodeType
{
    [Display(Name = "CurrencyCodeType_Toman", ResourceType = typeof(EnumResources))]
    Toman = 1,

    [Display(Name = "CurrencyCodeType_USD", ResourceType = typeof(EnumResources))]
    USD = 2,

    [Display(Name = "CurrencyCodeType_GBP", ResourceType = typeof(EnumResources))]
    GBP = 3,

    [Display(Name = "CurrencyCodeType_TRY", ResourceType = typeof(EnumResources))]
    TRY = 4,
}
