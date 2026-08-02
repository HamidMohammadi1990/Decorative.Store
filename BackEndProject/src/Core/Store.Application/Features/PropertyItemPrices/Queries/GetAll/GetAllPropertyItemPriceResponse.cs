using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public record GetAllPropertyItemPriceResponse
{
    [JsonConverter(typeof(PropertyItemPriceEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int PropertyItemId { get; init; }

    public string PropertyItemTitle { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }

    public string PropertyTitle { get; init; } = default!;

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public string PropertyCategoryTitle { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal CooperationPrice { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
}
