using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class DeleteBlogPostTagHandler
    (IUnitOfWork uow, IBlogPostTagRepository blogPostTagRepository)
    : IRequestHandler<DeleteBlogPostTagRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var blogPostTag = await blogPostTagRepository.FindAsync(request.Id);
        if (blogPostTag is null)
            return ErrorModel.Create("InvalidId");

        blogPostTagRepository.Remove(blogPostTag);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}