using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Orders.Common;

namespace Edition.Application.Features.Orders.Commands;

public record GetCartResponse
{
    [JsonConverter(typeof(OrderNullableEncryptor))]
    public int? OrderId { get; init; }

    public long? TrackingCode { get; init; }
    public OrderCartSummaryResponse Summary { get; init; } = new();
    public List<CartItemResponse> Items { get; init; } = [];
}

public record CartItemResponse
{
    [JsonConverter(typeof(OrderItemEncryptor))]
    public int OrderItemId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public string? ImageAlt { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string CurrencyCode { get; init; } = "IRT";
}
