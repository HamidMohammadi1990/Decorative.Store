using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Orders.Queries;

public record GetStatusSummaryOrderResponse
{
    [JsonConverter(typeof(OrderStatusTypeEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int Count { get; init; }
}