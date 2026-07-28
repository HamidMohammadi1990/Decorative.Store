using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFeatureTypes.Commands;

public record DeleteProductFeatureTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductFeatureTypeEncryptor))]
    public int Id { get; init; }
}
