using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public record UpdateDeliveryOptionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public int DeliveryDays { get; init; }
}