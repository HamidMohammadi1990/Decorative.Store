using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public record GetPropertyItemPriceRequest : IRequest<OperationResult<GetPropertyItemPriceResponse?>>
{
    [JsonConverter(typeof(PropertyItemPriceEncryptor))]
    public int Id { get; init; }
}