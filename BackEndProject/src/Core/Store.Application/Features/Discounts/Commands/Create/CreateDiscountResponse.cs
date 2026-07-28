using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Discounts.Commands;

public record CreateDiscountResponse
{
    [JsonConverter(typeof(DiscountEncryptor))]
    public int Id { get; init; }
}
