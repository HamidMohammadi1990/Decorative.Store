using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Cities.Queries;

public record GetCityRequest : IRequest<OperationResult<GetCityResponse>>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int Id { get; init; }
}