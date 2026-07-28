using Edition.Domain.Dtos.Others;

namespace Store.Domain.Dtos.Orders;

public record GetUserOrdersByStatusDto
{
    public int OrderId { get; init; }
    public long TrackingCode { get; init; }
    public string Title { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public decimal FinalPrice { get; init; } = default!;
    public string ProductImage { get; init; } = default!;
    public int OrderItemId { get; init; } = default!;    
    public int ItemQuantity { get; init; }
}