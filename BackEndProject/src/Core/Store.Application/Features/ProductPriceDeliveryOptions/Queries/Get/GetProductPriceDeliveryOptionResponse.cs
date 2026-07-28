using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public record GetProductPriceDeliveryOptionResponse
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int ProductPriceId { get; init; }

    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int DeliveryOptionId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
}