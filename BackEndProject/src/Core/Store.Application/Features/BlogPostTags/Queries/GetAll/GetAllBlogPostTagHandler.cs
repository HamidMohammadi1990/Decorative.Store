using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Queries;

public class GetAllBlogPostTagHandler
    (IBlogPostTagRepository blogPostTagRepository, IBlogPostTagMapperService mapper)
    : IRequestHandler<GetAllBlogPostTagRequest, OperationResult<PagedResult<GetAllBlogPostTagResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostTagResponse>>> Handle(GetAllBlogPostTagRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var tags = await blogPostTagRepository.GetAllAsync(requestModel);
        return mapper.Map(tags);
    }
}