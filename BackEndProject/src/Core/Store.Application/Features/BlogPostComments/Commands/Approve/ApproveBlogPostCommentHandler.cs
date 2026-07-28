using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class ApproveBlogPostCommentHandler
    (IUnitOfWork uow, IBlogPostCommentRepository blogPostCommentRepository, ICurrentUserContext currentUser)
    : IRequestHandler<ApproveBlogPostCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ApproveBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var blogPostComment = await blogPostCommentRepository.FindAsync(request.Id);
        if (blogPostComment is null)
            return ErrorModel.Create("InvalidId");

        var userId = currentUser.UserId;
        blogPostComment.Approve(userId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}