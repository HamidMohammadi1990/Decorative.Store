using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CommentTopics.Commands;

public class CreateCommentTopicHandler
    (IUnitOfWork uow, ICommentTopicRepository commentTopicRepository)
    : IRequestHandler<CreateCommentTopicRequest, OperationResult<CreateCommentTopicResponse>>
{
    public async Task<OperationResult<CreateCommentTopicResponse>> Handle(CreateCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var commentTopic = CommentTopic.Create(request.Title, request.Priority);
        commentTopicRepository.Add(commentTopic);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCommentTopicResponse>();

        return new CreateCommentTopicResponse { Id = commentTopic.Id };
    }
}