using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public record DeleteDeliveryTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(DeliveryTypeEncryptor))]
    public int Id { get; init; }
}
