using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Wallets.Commands;

public record ChargeWalletResponse
{
    [JsonConverter(typeof(BankTransactionNullableEncryptor))]
    public int? BankTransactionId { get; init; }

    public string? PaymentUrl { get; init; }
    public decimal Amount { get; init; }
}
