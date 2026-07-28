using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Tags.Commands;

public record CreateTagResponse
{
    [JsonConverter(typeof(TagEncryptor))]
    public int Id { get; init; }
}