using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Orders.Common;
using Store.Common.Models;

namespace Edition.Application.Features.Orders.Commands;

public record AddCartItemRequest : IRequest<OperationResult<GetCartResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public int Quantity { get; init; } = 1;
}
