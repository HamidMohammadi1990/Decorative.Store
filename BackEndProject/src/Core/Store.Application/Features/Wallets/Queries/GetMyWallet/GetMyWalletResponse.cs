using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Wallets.Queries;

public record GetMyWalletResponse
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public decimal Balance { get; init; }
    public bool IsDefault { get; init; }
    public WalletStatusType Status { get; init; }
    public DateTime CreatedOnUtc { get; init; }
}
