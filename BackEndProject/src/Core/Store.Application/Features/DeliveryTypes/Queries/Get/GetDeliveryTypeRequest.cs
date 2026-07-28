using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public record GetDeliveryTypeRequest : IRequest<OperationResult<GetDeliveryTypeResponse?>>
{
    [JsonConverter(typeof(DeliveryTypeEncryptor))]
    public int Id { get; init; }
}