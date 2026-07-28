using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public record GetOrderDetailResponse
{
	[JsonConverter(typeof(OrderEncryptor))]
	public int OrderId { get; set; }
	public long TrackingCode { get; set; }
	public string Title { get; set; } = default!;
	public OrderStatusType Status { get; init; }
	public bool IsFinaly { get; set; }
	public DateTime CreatedOnUtc { get; set; }
	public decimal TotalPrice { get; set; }
	public decimal FinalPrice { get; set; }

	public List<OrderItemResponse> Items { get; set; } = [];
}
public record OrderItemResponse
{
	[JsonConverter(typeof(OrderItemEncryptor))]
	public int Id { get; set; }
	public int Quantity { get; set; }
	public decimal? ProductPrice { get; set; }
	public bool IsNeedToDesign { get; set; }
	public string? EmergencyPhoneNumber { get; set; }
	public OrderItemStatusType Status { get; set; }
	public string? Description { get; set; }
	public DateTime CreatedOnUtc { get; set; }
	public string DeliveryType { get; set; } = default!;
	public string? PostType { get; set; }
	public OrderItemProductSummaryResponse Product { get; set; } = default!;
	public OrderItemCompanySummaryResponse Company { get; set; } = default!;
	public OrderItemUserAddressResponse UserAddress { get; set; } = default!;
	public List<OrderItemAttachmentResponse> Attachments { get; set; } = [];
	public List<OrderItemPropertyResponse> Properties { get; set; } = [];
}
public record OrderItemProductSummaryResponse
{
	[JsonConverter(typeof(ProductEncryptor))]
	public int Id { get; set; }
	public string Title { get; set; } = default!;
	public string Slug { get; set; } = default!;
	public string ProductCode { get; set; } = default!;
}
public class OrderItemCompanySummaryResponse
{
	[JsonConverter(typeof(CompanyEncryptor))]
	public int Id { get; set; }
	public string Name { get; set; } = default!;
	public string Code { get; set; } = default!;
	public string PhoneNumber { get; set; } = default!;
}
public record OrderItemUserAddressResponse
{
	public string Title { get; set; } = default!;
	public string CityTitle { get; set; } = default!;
	public string? RecipientFirstName { get; set; }
	public string? RecipientLastName { get; set; }
	public string Address { get; set; } = default!;
	public string? PostalCode { get; set; }
	public string PhoneNumber { get; set; } = default!;
}
public record OrderItemAttachmentResponse
{
	public string TypeTitle { get; set; } = default!;
	public string FileName { get; set; } = default!;
}
public record OrderItemPropertyResponse
{
	public string Title { get; set; } = default!;
	public decimal? Price { get; set; }
	public PropertyType PropertyType { get; set; }
}

public record BooleanOrderItemPropertyResponse : OrderItemPropertyResponse
{
	public bool IsSelected { get; set; }
}

public record NumericOrderItemPropertyResponse : OrderItemPropertyResponse
{
	public int Quantity { get; set; }
}

public record NumericWithItemOrderItemPropertyResponse : OrderItemPropertyResponse
{
	public int Quantity { get; set; }
	public string? ItemTitle { get; set; }
	public decimal? ItemPrice { get; set; }
}

public record SelectOrderItemPropertyResponse : OrderItemPropertyResponse
{
	public string? ItemTitle { get; set; }
	public decimal? ItemPrice { get; set; }
}

public record DimensionsOrderItemPropertyResponse : OrderItemPropertyResponse
{
	public decimal Width { get; set; }
	public decimal Height { get; set; }
}

public record TextOrderItemPropertyResponse : OrderItemPropertyResponse
{
    public string? Value { get; set; }
}