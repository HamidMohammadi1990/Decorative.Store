using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public record GetPropertyItemPriceResponse
{
    [JsonConverter(typeof(PropertyItemPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int PropertyItemId { get; init; }

    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
}
