using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserStoryComments.Commands;

public class ApproveUserStoryCommentHandler
    (
        IUnitOfWork uow,
        IUserStoryCommentRepository userStoryCommentRepository,
        ICurrentUserContext currentUser)
    : IRequestHandler<ApproveUserStoryCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ApproveUserStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await userStoryCommentRepository.FindAsync(request.Id, cancellationToken);
        if (comment is null)
            return ErrorModel.Create("InvalidId");

        comment.Approve(currentUser.UserId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess ? OperationResult.Success() : saveChangesResult;
    }
}
