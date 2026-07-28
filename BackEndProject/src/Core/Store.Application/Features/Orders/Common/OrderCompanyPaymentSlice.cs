namespace Edition.Application.Features.Orders.Common;

public sealed record OrderCompanyPaymentSlice(
    int CompanyId,
    decimal GrossAmount,
    decimal DiscountShare,
    decimal NetAmount,
    decimal VatAmount,
    decimal FinalAmount);
