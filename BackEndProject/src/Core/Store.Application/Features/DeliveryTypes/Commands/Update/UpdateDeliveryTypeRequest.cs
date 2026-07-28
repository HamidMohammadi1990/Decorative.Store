using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public record UpdateDeliveryTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(DeliveryTypeEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}
