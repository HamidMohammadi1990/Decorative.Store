using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class GetBlogPostCategoryHandler 
    (IBlogPostCategoryRepository blogPostCategoryRepository, IBlogPostCategoryMapperService mapper)
    : IRequestHandler<GetBlogPostCategoryRequest, OperationResult<GetBlogPostCategoryResponse?>>
{
    public async Task<OperationResult<GetBlogPostCategoryResponse?>> Handle(GetBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var blogPostCategory = await blogPostCategoryRepository.GetAsNoTrackingAsync(request.Id);
        if (blogPostCategory is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(blogPostCategory);
        return result;
    }
}