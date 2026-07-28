using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Queries;

public class SearchBlogPostTagHandler
    (IBlogPostTagRepository blogPostTagRepository, IBlogPostTagMapperService mapper)
    : IRequestHandler<SearchBlogPostTagRequest, OperationResult<PagedResult<SearchBlogPostTagResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBlogPostTagResponse>>> Handle(SearchBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var tags = await blogPostTagRepository.SearchAsync(requestModel);
        return mapper.Map(tags);
    }
}