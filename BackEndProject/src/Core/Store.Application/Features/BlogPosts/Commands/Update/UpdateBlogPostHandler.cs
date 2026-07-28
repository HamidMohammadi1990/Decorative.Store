using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Commands;

public class UpdateBlogPostHandler
    (IUnitOfWork uow, IBlogPostRepository blogPostRepository)
    : IRequestHandler<UpdateBlogPostRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var blogPost = await blogPostRepository.FindAsync(request.Id);
        if (blogPost is null)
            return ErrorModel.Create("InvalidId");

        blogPost.Update(request.Title, request.Slug, request.CategoryId, request.MetaDescription, request.SeoKeywords, request.Content, request.ReadingTimeInMinutes);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}