using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public record CreateProductPropertyPriceResponse
{
    [JsonConverter(typeof(ProductPropertyPriceEncryptor))]
    public int Id { get; init; }
}