using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PostTypes.Queries;

public record SearchPostTypeResponse
{
    [JsonConverter(typeof(PostTypeEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public int Priority { get; init; }
}