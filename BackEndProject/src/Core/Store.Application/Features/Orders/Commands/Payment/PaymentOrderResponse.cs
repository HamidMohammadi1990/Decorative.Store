using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Orders.Commands;

public record PaymentOrderResponse
{
    [JsonConverter(typeof(BankTransactionNullableEncryptor))]
    public int? BankTransactionId { get; init; }

    public string? PaymentUrl { get; init; }
    public bool IsBankPaymentRequired { get; init; }
    public decimal BankPaymentAmount { get; init; }
    public decimal WalletDeduction { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal FinalPrice { get; init; }
}
