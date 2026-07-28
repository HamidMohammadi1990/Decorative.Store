using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Orders.Common;

namespace Edition.Application.Features.Orders.Commands;

public record RemoveOrderDiscountResponse
{
    [JsonConverter(typeof(OrderEncryptor))]
    public int OrderId { get; init; }
    public OrderCartSummaryResponse Cart { get; init; } = default!;
}
