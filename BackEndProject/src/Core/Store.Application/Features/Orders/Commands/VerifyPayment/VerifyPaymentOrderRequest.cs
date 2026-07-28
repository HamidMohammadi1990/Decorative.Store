using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Commands;

public record VerifyPaymentOrderRequest : IRequest<OperationResult<VerifyPaymentOrderResponse>>
{
    [JsonConverter(typeof(BankTransactionNullableEncryptor))]
    public int? BankTransactionId { get; init; }

    public string? GatewayReference { get; init; }
}
