using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OrderCommissionStatusType
{
    [Display(Name = "OrderCommissionStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "OrderCommissionStatusType_Paid", ResourceType = typeof(EnumResources))]
    Paid = 2,

    [Display(Name = "OrderCommissionStatusType_Refunded", ResourceType = typeof(EnumResources))]
    Refunded = 3
}
