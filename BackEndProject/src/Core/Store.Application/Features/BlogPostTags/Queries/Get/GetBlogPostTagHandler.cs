using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Queries;

public class GetBlogPostTagHandler 
    (IBlogPostTagRepository blogPostTagRepository, IBlogPostTagMapperService mapper)
    : IRequestHandler<GetBlogPostTagRequest, OperationResult<GetBlogPostTagResponse?>>
{
    public async Task<OperationResult<GetBlogPostTagResponse?>> Handle(GetBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var blogPostTag = await blogPostTagRepository.GetAsNoTrackingAsync(request.Id);
        if (blogPostTag is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(blogPostTag);
        return result;
    }
}