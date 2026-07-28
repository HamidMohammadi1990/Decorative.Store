using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public record GetAllProductPriceDeliveryOptionResponse
{
    [JsonConverter(typeof(ProductPriceDeliveryOptionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductPriceEncryptor))]
    public int ProductPriceId { get; init; }

    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int DeliveryOptionId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;

    public string DeliveryOptionTitle { get; init; } = default!;

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string CompanyName { get; init; } = default!;

    public decimal Price { get; init; }

    public decimal CooperationPrice { get; init; }

    public DateTime CreatedOnUtc { get; init; }
}