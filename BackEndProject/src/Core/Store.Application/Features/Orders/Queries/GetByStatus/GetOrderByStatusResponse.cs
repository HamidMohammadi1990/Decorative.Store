using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Orders.Queries;

public record GetOrderByStatusResponse
{
    [JsonConverter(typeof(OrderEncryptor))]
    public int Id { get; init; }                           
    public long TrackingCode { get; init; }       
    public string Title { get; init; } = default!;   
    public DateTime CreatedOnUtc { get; init; }      
    public decimal FinalPrice { get; init; } = default!;

    public List<GetOrderByStatusItemResponse> Items { get; init; } = [];
}

public record GetOrderByStatusItemResponse
{
    [JsonConverter(typeof(OrderItemEncryptor))]
    public int Id { get; init; }    
    public int Quantity { get; init; }
    public string ProductImage { get; init; } = default!;
}
