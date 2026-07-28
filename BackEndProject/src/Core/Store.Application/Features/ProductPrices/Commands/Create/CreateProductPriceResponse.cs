using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPrices.Commands;

public record CreateProductPriceResponse
{
    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int Id { get; init; }
}