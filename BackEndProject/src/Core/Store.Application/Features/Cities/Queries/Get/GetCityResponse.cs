using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Cities.Queries;

public record GetCityResponse
{
    [JsonConverter(typeof(CityEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProvinceEncryptor))]
    public int ProvinceId { get; init; }

    public bool IsActive { get; init; }

    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? Description { get; init; }
    public int Rate { get; init; }
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
}