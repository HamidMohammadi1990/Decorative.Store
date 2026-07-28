using Store.Common.Models;

namespace Edition.Application.Features.CommentTopics.Commands;

public record CreateCommentTopicRequest : IRequest<OperationResult<CreateCommentTopicResponse>>
{
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
}