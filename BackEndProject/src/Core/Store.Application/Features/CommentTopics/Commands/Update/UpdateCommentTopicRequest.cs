using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CommentTopics.Commands;

public record UpdateCommentTopicRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}