using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostLikes.Commands;

public class CreateBlogPostLikeHandler
    (IUnitOfWork uow, IBlogPostLikeRepository blogPostLikeRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateBlogPostLikeRequest, OperationResult<CreateBlogPostLikeResponse>>
{
    public async Task<OperationResult<CreateBlogPostLikeResponse>> Handle(CreateBlogPostLikeRequest request, CancellationToken cancellationToken)
    {
        int? userId = null;
        if (currentUser.IsAuthenticated)
            userId = currentUser.UserId;

        var userIP = currentUser.ClientIp;

        var isExistsLike = await
            blogPostLikeRepository
            .AnyAsync(x => x.BlogPostId == request.BlogPostId && (x.UserId == userId || x.ClientIP == userIP));

        if (isExistsLike)
            return ErrorModel.Create("DuplicateLike");

        var blogPostLike = BlogPostLike.Create(request.BlogPostId, userIP, userId);
        blogPostLikeRepository.Add(blogPostLike);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBlogPostLikeResponse>();

        return new CreateBlogPostLikeResponse { Id = blogPostLike.Id };
    }
}