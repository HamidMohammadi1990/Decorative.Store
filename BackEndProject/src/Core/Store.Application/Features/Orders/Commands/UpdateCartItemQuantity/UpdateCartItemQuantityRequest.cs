using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Commands;

public record UpdateCartItemQuantityRequest : IRequest<OperationResult<GetCartResponse>>
{
    [JsonConverter(typeof(OrderItemEncryptor))]
    public int OrderItemId { get; init; }

    public int Quantity { get; init; }
}
