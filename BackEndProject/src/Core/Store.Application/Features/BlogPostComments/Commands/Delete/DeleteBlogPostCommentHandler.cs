using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class DeleteBlogPostCommentHandler
    (IUnitOfWork uow, IBlogPostCommentRepository blogPostCommentRepository)
    : IRequestHandler<DeleteBlogPostCommentRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var blogPostComment = await blogPostCommentRepository.FindAsync(request.Id);
        if (blogPostComment is null)
            return ErrorModel.Create("InvalidId");

        blogPostCommentRepository.Remove(blogPostComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}