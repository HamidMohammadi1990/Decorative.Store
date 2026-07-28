using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum AccountPartyType
{
    [Display(Name = "AccountPartyType_Customer", ResourceType = typeof(EnumResources))]
    Customer = 1,

    [Display(Name = "AccountPartyType_Intermediary", ResourceType = typeof(EnumResources))]
    Intermediary = 2,

    [Display(Name = "AccountPartyType_Company", ResourceType = typeof(EnumResources))]
    Company = 3,

    [Display(Name = "AccountPartyType_Vat", ResourceType = typeof(EnumResources))]
    Vat = 4
}
