using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public record SearchDeliveryOptionResponse
{
    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public int DeliveryDays { get; init; }
}