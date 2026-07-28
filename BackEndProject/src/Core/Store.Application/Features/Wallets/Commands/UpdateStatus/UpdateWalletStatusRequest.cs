using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Wallets.Commands;

public record UpdateWalletStatusRequest : IRequest<OperationResult<OperationResult>>
{
    [JsonConverter(typeof(WalletEncryptor))]
    public int Id { get; init; }

    public WalletStatusType Status { get; init; }
}
