using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public record CreateDeliveryOptionResponse
{
    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int Id { get; init; }
}