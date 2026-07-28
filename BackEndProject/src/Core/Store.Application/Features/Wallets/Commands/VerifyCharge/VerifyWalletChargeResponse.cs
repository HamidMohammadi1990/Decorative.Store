using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Wallets.Commands;

public record VerifyWalletChargeResponse
{
    public bool IsPaymentSuccessful { get; init; }

    [JsonConverter(typeof(WalletEncryptor))]
    public int WalletId { get; init; }

    public decimal Amount { get; init; }
    public decimal NewBalance { get; init; }
}
