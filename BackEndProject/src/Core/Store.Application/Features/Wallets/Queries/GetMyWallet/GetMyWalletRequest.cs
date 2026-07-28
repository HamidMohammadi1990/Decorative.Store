using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Queries;

public record GetMyWalletRequest : IRequest<OperationResult<GetMyWalletResponse?>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }
}
