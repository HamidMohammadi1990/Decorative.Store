using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyItems.Commands;

public record CreatePropertyItemResponse
{
    [JsonConverter(typeof(PropertyItemEncryptor))]
    public int Id { get; init; }
}