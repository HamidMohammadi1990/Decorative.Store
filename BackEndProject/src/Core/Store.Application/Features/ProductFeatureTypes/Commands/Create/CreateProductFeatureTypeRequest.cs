using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public record CreateProductFeatureTypeRequest : IRequest<OperationResult<CreateProductFeatureTypeResponse>>
{
    public string Name { get; init; } = default!;
    public ProductFeatureTypeCode Type { get; init; }
    public ProductFeatureDataType DataType { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
