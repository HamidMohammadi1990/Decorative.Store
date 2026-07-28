using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Commands;

public record RemoveOrderItemRequest : IRequest<OperationResult<RemoveOrderItemResponse>>
{
    [JsonConverter(typeof(OrderItemEncryptor))]
    public int OrderItemId { get; init; }
}
