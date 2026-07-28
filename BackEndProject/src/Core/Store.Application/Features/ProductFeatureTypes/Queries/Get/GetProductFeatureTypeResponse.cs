using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public record GetProductFeatureTypeResponse
{
    [JsonConverter(typeof(ProductFeatureTypeEncryptor))]
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public ProductFeatureTypeCode Type { get; init; }
    public ProductFeatureDataType DataType { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
