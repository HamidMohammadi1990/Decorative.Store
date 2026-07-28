using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public record CreatePropertyItemPriceResponse
{
    [JsonConverter(typeof(PropertyItemPriceEncryptor))]
    public int Id { get; init; }
}