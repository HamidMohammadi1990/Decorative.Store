using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Properties.Commands;

public record CreatePropertyResponse
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }
}