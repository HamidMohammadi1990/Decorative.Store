using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Wallets.Commands;

public record AdminChargeWalletResponse
{
    [JsonConverter(typeof(WalletTransactionEncryptor))]
    public int WalletTransactionId { get; init; }

    public decimal NewBalance { get; init; }
}
