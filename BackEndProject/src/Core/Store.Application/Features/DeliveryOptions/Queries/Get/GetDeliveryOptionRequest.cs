using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public record GetDeliveryOptionRequest : IRequest<OperationResult<GetDeliveryOptionResponse?>>
{
    [JsonConverter(typeof(DeliveryOptionEncryptor))]
    public int Id { get; init; }
}