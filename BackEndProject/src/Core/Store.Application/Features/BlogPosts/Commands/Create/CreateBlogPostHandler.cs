using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPosts.Commands;

public class CreateBlogPostHandler
    (IUnitOfWork uow, IBlogPostRepository blogPostRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateBlogPostRequest, OperationResult<CreateBlogPostResponse>>
{
    public async Task<OperationResult<CreateBlogPostResponse>> Handle(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var blogPost = BlogPost.Create(request.Title, request.Slug, request.CategoryId, request.MetaDescription,
                                       request.SeoKeywords, request.Content, userId, request.ReadingTimeInMinutes);

        blogPostRepository.Add(blogPost);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBlogPostResponse>();

        return new CreateBlogPostResponse { Id = blogPost.Id };
    }
}