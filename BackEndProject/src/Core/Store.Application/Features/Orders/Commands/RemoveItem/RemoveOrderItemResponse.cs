using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Orders.Common;

namespace Edition.Application.Features.Orders.Commands;

public record RemoveOrderItemResponse
{
    [JsonConverter(typeof(OrderNullableEncryptor))]
    public int? OrderId { get; init; }
    public long? TrackingCode { get; init; }
    public bool IsOrderDeleted { get; init; }
    public OrderCartSummaryResponse Cart { get; init; } = default!;
}
