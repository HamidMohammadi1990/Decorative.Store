using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class UpdateBlogPostTagHandler
    (IUnitOfWork uow, IBlogPostTagRepository blogPostTagRepository)
    : IRequestHandler<UpdateBlogPostTagRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var blogPostTag = await blogPostTagRepository.FindAsync(request.Id);
        if (blogPostTag is null)
            return ErrorModel.Create("InvalidId");

        blogPostTag.Update(request.TagId, request.BlogPostId);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}