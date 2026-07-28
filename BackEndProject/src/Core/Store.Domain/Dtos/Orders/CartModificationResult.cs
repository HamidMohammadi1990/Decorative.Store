using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public sealed record CartModificationResult
{
    public decimal TotalPrice { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
    public bool IsDiscountApplied { get; init; }
    public bool IsDiscountInvalidated { get; init; }
    public DiscountValidationFailure? InvalidationReason { get; init; }

    public static CartModificationResult Empty()
        => new()
        {
            TotalPrice = 0,
            FinalPrice = 0,
            DiscountAmount = 0,
            IsDiscountApplied = false,
            IsDiscountInvalidated = false
        };

    public static CartModificationResult WithoutDiscount(decimal totalPrice)
        => new()
        {
            TotalPrice = totalPrice,
            FinalPrice = totalPrice,
            DiscountAmount = 0,
            IsDiscountApplied = false,
            IsDiscountInvalidated = false
        };

    public static CartModificationResult WithDiscount(decimal totalPrice, decimal discountAmount)
        => new()
        {
            TotalPrice = totalPrice,
            DiscountAmount = discountAmount,
            FinalPrice = totalPrice - discountAmount,
            IsDiscountApplied = true,
            IsDiscountInvalidated = false
        };

    public static CartModificationResult DiscountInvalidated(
        decimal totalPrice,
        DiscountValidationFailure reason)
        => new()
        {
            TotalPrice = totalPrice,
            FinalPrice = totalPrice,
            DiscountAmount = 0,
            IsDiscountApplied = false,
            IsDiscountInvalidated = true,
            InvalidationReason = reason
        };
}
