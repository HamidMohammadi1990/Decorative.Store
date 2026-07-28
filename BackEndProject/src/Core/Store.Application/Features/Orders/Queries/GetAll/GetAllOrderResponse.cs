using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.Orders.Queries;

public record GetAllOrderResponse
{
    [JsonConverter(typeof(OrderEncryptor))]
    public int Id { get; init; }

    public long TrackingCode { get; init; }
    public string Title { get; init; } = null!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserFirstName { get; init; } = default!;
    public string UserLastName { get; init; } = default!;
    public OrderStatusType Status { get; init; }
    public bool IsFinaly { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal FinalPrice { get; init; }
    public decimal VatPrice { get; init; }
    public decimal TotalCommissionPrice { get; init; }
}