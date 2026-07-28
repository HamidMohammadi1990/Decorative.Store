using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Common;

public readonly record struct OrderPaymentAmounts(decimal WalletDeduction, decimal BankPaymentAmount)
{
    public decimal Total => WalletDeduction + BankPaymentAmount;

    public static OrderPaymentAmounts Calculate(
        PaymentOptionType paymentOption,
        decimal finalPrice,
        decimal walletBalance)
    {
        return paymentOption switch
        {
            PaymentOptionType.BankOnly => new(0m, finalPrice),
            PaymentOptionType.WalletOnly => new(finalPrice, 0m),
            PaymentOptionType.WalletAndBank => new(
                Math.Min(walletBalance, finalPrice),
                Math.Max(0m, finalPrice - Math.Min(walletBalance, finalPrice))),
            _ => new(0m, finalPrice)
        };
    }
}
