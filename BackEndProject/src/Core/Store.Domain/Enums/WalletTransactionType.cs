using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum WalletTransactionType
{
    [Display(Name = "WalletTransactionType_Incremental", ResourceType = typeof(EnumResources))]
    Incremental,

    [Display(Name = "WalletTransactionType_Decremental", ResourceType = typeof(EnumResources))]
    Decremental
}
