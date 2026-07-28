using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Provinces.Commands;

public record CreateProvinceResponse
{
    [JsonConverter(typeof(ProvinceEncryptor))]
    public int Id { get; init; }
}