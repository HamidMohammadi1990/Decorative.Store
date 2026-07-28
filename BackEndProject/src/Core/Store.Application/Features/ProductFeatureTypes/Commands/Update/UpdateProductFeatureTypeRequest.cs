using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public record UpdateProductFeatureTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductFeatureTypeEncryptor))]
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public ProductFeatureTypeCode Type { get; init; }
    public ProductFeatureDataType DataType { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
