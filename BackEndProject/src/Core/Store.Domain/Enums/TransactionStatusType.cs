using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum TransactionStatusType
{
    [Display(Name = "TransactionStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "TransactionStatusType_Completed", ResourceType = typeof(EnumResources))]
    Completed = 2,

    [Display(Name = "TransactionStatusType_Failed", ResourceType = typeof(EnumResources))]
    Failed = 3,

    [Display(Name = "TransactionStatusType_Canceled", ResourceType = typeof(EnumResources))]
    Canceled = 4,

    [Display(Name = "TransactionStatusType_Refunded", ResourceType = typeof(EnumResources))]
    Refunded = 5,

    [Display(Name = "TransactionStatusType_Partial", ResourceType = typeof(EnumResources))]
    Partial = 6
}
