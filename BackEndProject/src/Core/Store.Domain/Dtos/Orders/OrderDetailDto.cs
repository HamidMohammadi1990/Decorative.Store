using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public record OrderDetailDto
{
    public int Id { get; init; }
    public long TrackingCode { get; init; } = default!;
    public string Title { get; init; } = default!;
    public OrderStatusType Status { get; init; }
    public bool IsFinaly { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public decimal TotalPrice { get; init; } = default!;
    public decimal FinalPrice { get; init; } = default!;

    public List<OrderDetailItemDto> Items { get; init; } = [];
}
public record OrderDetailItemDto
{
    public int Id { get; init; }
    public int Quantity { get; init; }
    public decimal? ProductPrice { get; init; }
    public bool IsNeedToDesign { get; init; }
    public string? EmergencyPhoneNumber { get; init; }
    public string? Description { get; init; }
    public OrderItemStatusType Status { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public string? DeliveryTypeTitle { get; init; }
    public string? PostTypeTitle { get; init; }
    public OrderItemProductSummaryDto Product { get; init; } = default!;
    public OrderItemUserAddressDto UserAddress { get; init; } = default!;

    public List<OrderItemAttachmentDetailDto> Attachments { get; init; } = [];
    public List<OrderItemPropertyDetailDto> Properties { get; init; } = [];
}
public record OrderItemProductSummaryDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string ProductCode { get; init; } = default!;
}
public record OrderItemAttachmentDetailDto
{
    public string TypeTitle { get; init; } = default!;
    public string FileName { get; init; } = default!;
}
public record OrderItemPropertyDetailDto
{
    public string Title { get; init; } = default!;
    public decimal? Price { get; init; }
    public string? Value { get; init; }
    public string? ItemTitle { get; init; } = default!;
    public decimal? ItemPrice { get; init; }
    public PropertyType PropertyType { get; init; }
    public bool? IsSelected { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
    public int? Quantity { get; init; }
}
public record OrderItemUserAddressDto
{
    public string Title { get; init; } = default!;
    public string CityTitle { get; init; } = default!;
    public string? RecipientFirstName { get; init; }
    public string? RecipientLastName { get; init; }
    public string Address { get; init; } = default!;
    public string? PostalCode { get; init; }
    public string PhoneNumber { get; init; } = default!;
}