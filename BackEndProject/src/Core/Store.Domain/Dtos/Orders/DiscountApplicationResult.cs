using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public sealed record DiscountApplicationResult
{
    public bool IsSuccess { get; init; }
    public decimal DiscountAmount { get; init; }
    public DiscountValidationFailure? Failure { get; init; }

    public static DiscountApplicationResult Successful(decimal discountAmount)
        => new() { IsSuccess = true, DiscountAmount = discountAmount };

    public static DiscountApplicationResult Failed(DiscountValidationFailure failure)
        => new() { IsSuccess = false, Failure = failure };
}
