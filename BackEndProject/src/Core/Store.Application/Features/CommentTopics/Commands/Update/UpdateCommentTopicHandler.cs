using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CommentTopics.Commands;

public class UpdateCommentTopicHandler
    (ICommentTopicRepository commentTopicRepository, IUnitOfWork uow)
    : IRequestHandler<UpdateCommentTopicRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var commentTopic = await commentTopicRepository.FindAsync(request.Id);
        if (commentTopic is null)
            return ErrorModel.Create("InvalidId");

        commentTopic.Update(request.Title, request.Priority, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}