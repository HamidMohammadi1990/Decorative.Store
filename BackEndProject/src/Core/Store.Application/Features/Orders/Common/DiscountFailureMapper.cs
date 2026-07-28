using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Common;

public static class DiscountFailureMapper
{
    public static ErrorModel ToErrorModel(DiscountValidationFailure failure)
        => ErrorModel.Create(GetCode(failure));

    private static string GetCode(DiscountValidationFailure failure)
        => failure switch
        {
            DiscountValidationFailure.Inactive => "DiscountInactive",
            DiscountValidationFailure.Expired => "DiscountExpired",
            DiscountValidationFailure.CooperationOnly => "DiscountCooperationOnly",
            DiscountValidationFailure.UsageLimitReached => "DiscountUsageLimitReached",
            DiscountValidationFailure.UserNotEligible => "DiscountUserNotEligible",
            DiscountValidationFailure.MinimumAmountNotMet => "DiscountMinimumAmountNotMet",
            DiscountValidationFailure.NoEligibleItems => "DiscountNoEligibleItems",
            DiscountValidationFailure.InvalidDiscountType => "DiscountInvalidType",
            DiscountValidationFailure.DiscountAlreadyConsumed => "DiscountAlreadyConsumed",
            _ => "DiscountIsNotValid"
        };
}