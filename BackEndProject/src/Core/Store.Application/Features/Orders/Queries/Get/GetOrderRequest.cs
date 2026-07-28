using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Queries;

public record GetOrderRequest : IRequest<OperationResult<GetOrderResponse?>>
{
    [JsonConverter(typeof(OrderEncryptor))]
    public int Id { get; init; }
}