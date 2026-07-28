using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public class GetBlogPostLikeHandler
    (IBlogPostLikeRepository blogPostLikeRepository, IBlogPostLikeMapperService mapper)
    : IRequestHandler<GetBlogPostLikeRequest, OperationResult<GetBlogPostLikeResponse?>>
{
    public async Task<OperationResult<GetBlogPostLikeResponse?>> Handle(GetBlogPostLikeRequest request, CancellationToken cancellationToken)
    {
        var blogPostLike = await blogPostLikeRepository.GetAsNoTrackingAsync(request.Id);
        if (blogPostLike is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(blogPostLike);
        return result;
    }
}