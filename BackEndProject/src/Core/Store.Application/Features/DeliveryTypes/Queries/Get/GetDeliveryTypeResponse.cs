using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public record GetDeliveryTypeResponse
{
    [JsonConverter(typeof(DeliveryTypeEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
    public int Priority { get; init; }
}