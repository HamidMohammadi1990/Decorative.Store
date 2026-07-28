using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public record GetProductFeatureTypeRequest : IRequest<OperationResult<GetProductFeatureTypeResponse?>>
{
    [JsonConverter(typeof(ProductFeatureTypeEncryptor))]
    public int Id { get; init; }
}
