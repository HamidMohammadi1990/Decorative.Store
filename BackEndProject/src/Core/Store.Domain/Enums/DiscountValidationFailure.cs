using System.ComponentModel.DataAnnotations;
using Store.Domain.Resources;

namespace Store.Domain.Enums;

public enum DiscountValidationFailure
{
    [Display(Name = "DiscountValidationFailure_Inactive", ResourceType = typeof(EnumResources))]
    Inactive = 1,

    [Display(Name = "DiscountValidationFailure_Expired", ResourceType = typeof(EnumResources))]
    Expired = 2,

    [Display(Name = "DiscountValidationFailure_CooperationOnly", ResourceType = typeof(EnumResources))]
    CooperationOnly = 3,

    [Display(Name = "DiscountValidationFailure_UsageLimitReached", ResourceType = typeof(EnumResources))]
    UsageLimitReached = 4,

    [Display(Name = "DiscountValidationFailure_UserNotEligible", ResourceType = typeof(EnumResources))]
    UserNotEligible = 5,

    [Display(Name = "DiscountValidationFailure_MinimumAmountNotMet", ResourceType = typeof(EnumResources))]
    MinimumAmountNotMet = 6,

    [Display(Name = "DiscountValidationFailure_NoEligibleItems", ResourceType = typeof(EnumResources))]
    NoEligibleItems = 7,

    [Display(Name = "DiscountValidationFailure_InvalidDiscountType", ResourceType = typeof(EnumResources))]
    InvalidDiscountType = 8,

    [Display(Name = "DiscountValidationFailure_DiscountAlreadyConsumed", ResourceType = typeof(EnumResources))]
    DiscountAlreadyConsumed = 9
}
