using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Wallets.Commands;

public record VerifyWalletChargeRequest : IRequest<OperationResult<VerifyWalletChargeResponse>>
{
    [JsonConverter(typeof(BankTransactionNullableEncryptor))]
    public int? BankTransactionId { get; init; }

    public string? GatewayReference { get; init; }
}
