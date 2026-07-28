using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Commands;

public class PublishBlogPostHandler
    (IBlogPostRepository blogPostRepository, IUnitOfWork uow)
    : IRequestHandler<PublishBlogPostRequest, OperationResult>
{
    public async Task<OperationResult> Handle(PublishBlogPostRequest request, CancellationToken cancellationToken)
    {
        var blogPost = await blogPostRepository.FindAsync(request.Id);
        if (blogPost is null)
            return ErrorModel.Create("InvalidId");

        blogPost.Publish();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}