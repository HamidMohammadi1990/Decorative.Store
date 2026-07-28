using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public record GetStatusSummaryPropertiesDto
{
    public OrderStatusType Status { get; init; }
    public int Count { get; init; }
}