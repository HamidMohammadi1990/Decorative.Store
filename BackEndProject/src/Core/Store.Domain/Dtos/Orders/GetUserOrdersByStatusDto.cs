using Store.Domain.Dtos.Others;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public record GetUserOrdersByStatusDto
{
    public int OrderId { get; init; }
    public long TrackingCode { get; init; }
    public string Title { get; init; } = default!;
    public OrderStatusType Status { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public decimal FinalPrice { get; init; } = default!;
    public string ProductImage { get; init; } = default!;
    public string ProductTitle { get; init; } = default!;
    public decimal? ProductPrice { get; init; }
    public int OrderItemId { get; init; } = default!;
    public int ItemQuantity { get; init; }
}