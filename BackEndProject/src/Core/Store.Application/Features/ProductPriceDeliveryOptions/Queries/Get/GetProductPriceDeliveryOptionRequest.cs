using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public record GetProductPriceDeliveryOptionRequest : IRequest<OperationResult<GetProductPriceDeliveryOptionResponse?>>
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }
}