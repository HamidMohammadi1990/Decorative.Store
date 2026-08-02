using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public record GetAllProductPropertyPriceResponse
{
    [JsonConverter(typeof(ProductPropertyPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int ProductPropertyId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;
}
