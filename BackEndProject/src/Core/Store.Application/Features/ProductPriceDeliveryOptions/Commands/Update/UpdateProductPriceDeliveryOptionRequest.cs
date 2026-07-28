using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public record UpdateProductPriceDeliveryOptionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int ProductPriceId { get; init; }

    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int DeliveryOptionId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
}