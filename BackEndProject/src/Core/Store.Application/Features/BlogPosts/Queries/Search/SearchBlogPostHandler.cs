using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class SearchBlogPostHandler
    (IBlogPostRepository blogPostRepository, IBlogPostMapperService mapper)
    : IRequestHandler<SearchBlogPostRequest, OperationResult<PagedResult<SearchBlogPostResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBlogPostResponse>>> Handle(SearchBlogPostRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var posts = await blogPostRepository.SearchAsync(requestModel);
        return mapper.Map(posts);
    }
}
