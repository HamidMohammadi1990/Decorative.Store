using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPrices.Queries;

public record GetProductPriceRequest : IRequest<OperationResult<GetProductPriceResponse?>>
{
    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int Id { get; init; }
}