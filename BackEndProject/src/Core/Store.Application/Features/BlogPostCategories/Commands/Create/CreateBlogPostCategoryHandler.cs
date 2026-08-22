using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public class CreateBlogPostCategoryHandler
    (IUnitOfWork uow, IBlogPostCategoryRepository blogPostCategoryRepository)
    : IRequestHandler<CreateBlogPostCategoryRequest, OperationResult<CreateBlogPostCategoryResponse>>
{
    public async Task<OperationResult<CreateBlogPostCategoryResponse>> Handle(CreateBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var blogPostCategory = BlogPostCategory.Create(request.Code);
        blogPostCategory.UpsertTranslation(request.LanguageId, request.Title, request.Slug);
        blogPostCategoryRepository.Add(blogPostCategory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateBlogPostCategoryResponse>();

        return new CreateBlogPostCategoryResponse { Id = blogPostCategory.Id };
    }
}
