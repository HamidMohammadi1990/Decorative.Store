using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Cities.Commands;

public record CreateCityRequest : IRequest<OperationResult<CreateCityResponse>>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int ProvinceId { get; init; }

    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? Description { get; init; }
    public int Rate { get; init; }
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
}