using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Cities.Commands;

public record UpdateCityRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProvinceEncryptor))]
    public int ProvinceId { get; init; }

    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? Description { get; init; }
    public int Rate { get; init; }
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }    
}