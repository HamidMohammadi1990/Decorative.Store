using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class CreateBlogPostTagHandler
    (IUnitOfWork uow, IBlogPostTagRepository blogPostTagRepository)
    : IRequestHandler<CreateBlogPostTagRequest, OperationResult<CreateBlogPostTagResponse>>
{
    public async Task<OperationResult<CreateBlogPostTagResponse>> Handle(CreateBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var blogPostTag = BlogPostTag.Create(request.TagId, request.BlogPostId);

        blogPostTagRepository.Add(blogPostTag);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBlogPostTagResponse>();

        return new CreateBlogPostTagResponse { Id = blogPostTag.Id };
    }
}