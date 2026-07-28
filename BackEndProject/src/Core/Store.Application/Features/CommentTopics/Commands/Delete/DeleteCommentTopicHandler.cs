using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CommentTopics.Commands;

public class DeleteCommentTopicHandler
    (ICommentTopicRepository commentTopicRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteCommentTopicRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var commentTopic = await commentTopicRepository.FindAsync(request.Id);
        if (commentTopic is null)
            return ErrorModel.Create("InvalidId");

        commentTopicRepository.Remove(commentTopic);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
