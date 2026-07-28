using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public record GetProductPropertyPriceRequest : IRequest<OperationResult<GetProductPropertyPriceResponse?>>
{
    [JsonConverter(typeof(ProductPropertyPriceEncryptor))]
    public int Id { get; init; }
}