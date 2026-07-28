using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class SearchBlogPostCategoryHandler
    (IBlogPostCategoryRepository blogPostCategoryRepository, IBlogPostCategoryMapperService mapper)
    : IRequestHandler<SearchBlogPostCategoryRequest, OperationResult<PagedResult<SearchBlogPostCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBlogPostCategoryResponse>>> Handle(SearchBlogPostCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var categories = await blogPostCategoryRepository.SearchAsync(requestModel);
        return mapper.Map(categories);
    }
}