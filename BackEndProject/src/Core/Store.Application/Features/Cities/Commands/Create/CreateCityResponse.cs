using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Cities.Commands;

public record CreateCityResponse
{
    [JsonConverter(typeof(CityEncryptor))]
    public int Id { get; init; }
}