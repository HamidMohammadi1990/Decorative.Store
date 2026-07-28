using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CommentTopics.Queries;

public record GetAllCommentTopicResponse
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}