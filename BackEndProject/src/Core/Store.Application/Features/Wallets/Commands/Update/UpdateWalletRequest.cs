using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Commands;

public record UpdateWalletRequest : IRequest<OperationResult<OperationResult>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public bool IsDefault { get; init; }
}
