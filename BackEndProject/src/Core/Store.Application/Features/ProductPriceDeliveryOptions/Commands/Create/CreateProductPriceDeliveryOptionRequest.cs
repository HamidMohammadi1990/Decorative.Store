using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public record CreateProductPriceDeliveryOptionRequest : IRequest<OperationResult<CreateProductPriceDeliveryOptionResponse>>
{
    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int ProductPriceId { get; init; }

    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int DeliveryOptionId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
}