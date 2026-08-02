using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Commands;

public record CreateWalletRequest : IRequest<OperationResult<CreateWalletResponse>>
{
    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string Title { get; init; } = default!;
    public bool IsDefault { get; init; }
}
