using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public record DeleteProductPriceDeliveryOptionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }
}