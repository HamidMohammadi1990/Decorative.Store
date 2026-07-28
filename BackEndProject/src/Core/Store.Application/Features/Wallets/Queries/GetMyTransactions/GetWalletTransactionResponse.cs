using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Wallets.Queries;

public record GetWalletTransactionResponse
{
    [JsonConverter(typeof(WalletTransactionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(WalletEncryptor))]
    public int WalletId { get; init; }

    public decimal Amount { get; init; }
    public string Description { get; init; } = default!;
    public WalletTransactionType Type { get; init; }
    public WalletTransactionStatusType Status { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}
