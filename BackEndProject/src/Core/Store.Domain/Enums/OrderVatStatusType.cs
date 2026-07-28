using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OrderVatStatusType
{
    [Display(Name = "OrderVatStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "OrderVatStatusType_Paid", ResourceType = typeof(EnumResources))]
    Paid = 2,

    [Display(Name = "OrderVatStatusType_Refunded", ResourceType = typeof(EnumResources))]
    Refunded = 3
}
