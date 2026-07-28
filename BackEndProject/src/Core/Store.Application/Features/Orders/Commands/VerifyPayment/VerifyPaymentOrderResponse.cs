using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Commands;

public record VerifyPaymentOrderResponse
{
    public bool IsPaymentSuccessful { get; init; }
    public long TrackingCode { get; init; }
    public OrderStatusType Status { get; init; }
    public decimal FinalPrice { get; init; }
    public decimal WalletDeduction { get; init; }
    public decimal BankPaymentAmount { get; init; }
}
