namespace Edition.Application.Features.Orders.Common;

public sealed record OrderPaymentSlice(
    decimal GrossAmount,
    decimal DiscountAmount,
    decimal NetAmount,
    decimal VatAmount,
    decimal FinalAmount);
