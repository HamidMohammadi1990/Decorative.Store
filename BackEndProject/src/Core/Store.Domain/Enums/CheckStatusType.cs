using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum CheckStatusType
{
    [Display(Name = "CheckStatusType_Unpaid", ResourceType = typeof(EnumResources))]
    Unpaid = 1,

    [Display(Name = "CheckStatusType_Paid", ResourceType = typeof(EnumResources))]
    Paid = 2,

    [Display(Name = "CheckStatusType_InsufficientFunds", ResourceType = typeof(EnumResources))]
    InsufficientFunds = 3,

    [Display(Name = "CheckStatusType_Blocked", ResourceType = typeof(EnumResources))]
    Blocked = 4,

    [Display(Name = "CheckStatusType_Expired", ResourceType = typeof(EnumResources))]
    Expired = 5,

    [Display(Name = "CheckStatusType_Seized", ResourceType = typeof(EnumResources))]
    Seized = 6,

    [Display(Name = "CheckStatusType_Lost", ResourceType = typeof(EnumResources))]
    Lost = 7,

    [Display(Name = "CheckStatusType_Valid", ResourceType = typeof(EnumResources))]
    Valid = 8,

    [Display(Name = "CheckStatusType_Cancelled", ResourceType = typeof(EnumResources))]
    Cancelled = 9,

    [Display(Name = "CheckStatusType_SentToBank", ResourceType = typeof(EnumResources))]
    SentToBank = 10
}
