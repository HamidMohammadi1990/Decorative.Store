using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CommentTopics.Queries;

public record GetCommentTopicRequest : IRequest<OperationResult<GetCommentTopicResponse>>
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int Id { get; init; }
}