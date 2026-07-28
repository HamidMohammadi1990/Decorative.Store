using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public class GetAllBlogPostLikeHandler
    (IBlogPostLikeRepository blogPostLikeRepository, IBlogPostLikeMapperService mapper)
    : IRequestHandler<GetAllBlogPostLikeRequest, OperationResult<PagedResult<GetAllBlogPostLikeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostLikeResponse>>> Handle(GetAllBlogPostLikeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var blogPostLikes = await blogPostLikeRepository.GetAllAsync(requestModel);
        return mapper.Map(blogPostLikes);
    }
}