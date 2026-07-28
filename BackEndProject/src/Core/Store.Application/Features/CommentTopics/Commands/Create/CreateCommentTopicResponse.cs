using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CommentTopics.Commands;

public record CreateCommentTopicResponse
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int Id { get; init; }
}