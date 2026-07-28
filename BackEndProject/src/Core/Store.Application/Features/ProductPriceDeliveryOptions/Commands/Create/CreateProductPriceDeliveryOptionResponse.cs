using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public record CreateProductPriceDeliveryOptionResponse
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }
}