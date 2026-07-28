using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Tags.Queries;

public record GetTagResponse
{
    [JsonConverter(typeof(TagEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
}