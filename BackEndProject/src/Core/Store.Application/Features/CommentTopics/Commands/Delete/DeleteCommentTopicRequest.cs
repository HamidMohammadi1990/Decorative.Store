using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CommentTopics.Commands;

public record DeleteCommentTopicRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int Id { get; init; }
}
