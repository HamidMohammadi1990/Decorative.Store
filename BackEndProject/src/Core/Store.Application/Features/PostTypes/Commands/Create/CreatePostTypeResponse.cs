using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PostTypes.Commands;

public record CreatePostTypeResponse
{
    [JsonConverter(typeof(PostTypeEncryptor))]
    public int Id { get; init; }
}