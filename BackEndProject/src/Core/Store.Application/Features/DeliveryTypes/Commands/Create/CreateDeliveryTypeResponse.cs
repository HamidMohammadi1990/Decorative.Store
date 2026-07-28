using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public record CreateDeliveryTypeResponse
{
    [JsonConverter(typeof(DeliveryTypeEncryptor))]
    public int Id { get; init; }
}