using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Cities.Commands;

public record DeleteCityRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CityEncryptor))]
    public int Id { get; init; }
}