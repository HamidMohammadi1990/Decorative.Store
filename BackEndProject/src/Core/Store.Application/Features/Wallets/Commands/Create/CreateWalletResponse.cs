using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Wallets.Commands;

public record CreateWalletResponse
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }
}
