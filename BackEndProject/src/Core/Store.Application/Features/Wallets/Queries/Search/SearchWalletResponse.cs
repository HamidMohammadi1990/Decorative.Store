using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Wallets.Queries;

public record SearchWalletResponse
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public decimal Balance { get; init; }
    public bool IsDefault { get; init; }
}
