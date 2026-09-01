using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum MarketingPromoType
{
    [Display(Name = "MarketingPromoType_Sale", ResourceType = typeof(EnumResources))]
    Sale = 1,

    [Display(Name = "MarketingPromoType_NewArrival", ResourceType = typeof(EnumResources))]
    NewArrival = 2,
}
