using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum RefundMethodType
{
    [Display(Name = "RefundMethodType_Wallet", ResourceType = typeof(EnumResources))]
    Wallet = 1,

    [Display(Name = "RefundMethodType_BankAccount", ResourceType = typeof(EnumResources))]
    BankAccount = 2
}
