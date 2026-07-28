using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class CreateBlogPostCommentHandler
    (IUnitOfWork uow, IBlogPostCommentRepository blogPostCommentRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateBlogPostCommentRequest, OperationResult<CreateBlogPostCommentResponse>>
{
    public async Task<OperationResult<CreateBlogPostCommentResponse>> Handle(CreateBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        int userId = currentUser.UserId;
        var blogPostComment = BlogPostComment.Create(request.ParentId, request.Content, userId, request.BlogPostId);

        blogPostCommentRepository.Add(blogPostComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBlogPostCommentResponse>();

        return new CreateBlogPostCommentResponse { Id = blogPostComment.Id };
    }
}