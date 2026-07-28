using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum WalletTransactionStatusType
{
    [Display(Name = "WalletTransactionStatusType_Pending", ResourceType = typeof(EnumResources))]
    Pending = 1,

    [Display(Name = "WalletTransactionStatusType_Completed", ResourceType = typeof(EnumResources))]
    Completed = 2,

    [Display(Name = "WalletTransactionStatusType_Failed", ResourceType = typeof(EnumResources))]
    Failed = 3
}
