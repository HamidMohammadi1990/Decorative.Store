using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class GetAllBlogPostCategoryHandler
    (IBlogPostCategoryRepository blogPostCategoryRepository, IBlogPostCategoryMapperService mapper)
    : IRequestHandler<GetAllBlogPostCategoryRequest, OperationResult<PagedResult<GetAllBlogPostCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostCategoryResponse>>> Handle(GetAllBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var categories = await blogPostCategoryRepository.GetAllAsync(requestModel);
        return mapper.Map(categories);
    }
}