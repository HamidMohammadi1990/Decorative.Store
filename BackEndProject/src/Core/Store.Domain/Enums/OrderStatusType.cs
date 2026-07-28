using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum OrderStatusType
{
    [Display(Name = "OrderStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "OrderStatusType_Rejected", ResourceType = typeof(EnumResources))]
    Rejected = 2,

    [Display(Name = "OrderStatusType_InProgress", ResourceType = typeof(EnumResources))]
    InProgress = 3,

    [Display(Name = "OrderStatusType_Completed", ResourceType = typeof(EnumResources))]
    Completed = 4
}
