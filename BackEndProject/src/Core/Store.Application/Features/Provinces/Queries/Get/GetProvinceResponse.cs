using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Provinces.Queries;

public record GetProvinceResponse
{
    [JsonConverter(typeof(ProvinceEncryptor))]
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public int Rate { get; init; }
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
}