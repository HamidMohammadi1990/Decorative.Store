using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class UpdateBlogPostCommentHandler
    (IUnitOfWork uow, IBlogPostCommentRepository blogPostCommentRepository)
    : IRequestHandler<UpdateBlogPostCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var blogPostComment = await blogPostCommentRepository.FindAsync(request.Id);
        if (blogPostComment is null)
            return ErrorModel.Create("InvalidId");

        blogPostComment.Update(request.ParentId, request.Content, request.BlogPostId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}