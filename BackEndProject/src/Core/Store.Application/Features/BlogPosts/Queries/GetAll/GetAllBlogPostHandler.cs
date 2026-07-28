using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetAllBlogPostHandler
    (IBlogPostRepository blogPostRepository, IBlogPostMapperService mapper)
    : IRequestHandler<GetAllBlogPostRequest, OperationResult<PagedResult<GetAllBlogPostResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostResponse>>> Handle(GetAllBlogPostRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var posts = await blogPostRepository.GetAllAsync(requestModel);
        return mapper.Map(posts);
    }
}
