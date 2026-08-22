using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public class UpdateBlogPostCategoryHandler
    (IUnitOfWork uow, IBlogPostCategoryRepository blogPostCategoryRepository)
    : IRequestHandler<UpdateBlogPostCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var blogPostCategory = await blogPostCategoryRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (blogPostCategory is null)
            return ErrorModel.Create("InvalidId");

        blogPostCategory.Update(request.Code, request.IsActive, request.LanguageId, request.Title, request.Slug);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
