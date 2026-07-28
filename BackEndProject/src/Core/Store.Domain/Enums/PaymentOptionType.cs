using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum PaymentOptionType
{
    [Display(Name = "PaymentOptionType_BankOnly", ResourceType = typeof(EnumResources))]
    BankOnly = 1,

    [Display(Name = "PaymentOptionType_WalletOnly", ResourceType = typeof(EnumResources))]
    WalletOnly = 2,

    [Display(Name = "PaymentOptionType_WalletAndBank", ResourceType = typeof(EnumResources))]
    WalletAndBank = 3
}
