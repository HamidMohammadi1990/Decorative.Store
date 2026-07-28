using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetBlogPostHandler
    (IBlogPostRepository blogPostRepository, IBlogPostMapperService mapper)
    : IRequestHandler<GetBlogPostRequest, OperationResult<GetBlogPostResponse?>>
{
    public async Task<OperationResult<GetBlogPostResponse?>> Handle(GetBlogPostRequest request, CancellationToken cancellationToken)
    {
        var blogPost = await blogPostRepository.GetAsNoTrackingAsync(request.Id);
        if (blogPost is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(blogPost);
        return result;
    }
}