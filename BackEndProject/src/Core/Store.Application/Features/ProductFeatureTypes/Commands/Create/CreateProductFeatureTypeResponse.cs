using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public record CreateProductFeatureTypeResponse
{
    [JsonConverter(typeof(ProductFeatureTypeEncryptor))]
    public int Id { get; init; }
}
