using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class GetAllBlogPostCommentHandler
    (IBlogPostCommentRepository blogPostCommentRepository, IBlogPostCommentMapperService mapper)
    : IRequestHandler<GetAllBlogPostCommentRequest, OperationResult<PagedResult<GetAllBlogPostCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllBlogPostCommentResponse>>> Handle(GetAllBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await blogPostCommentRepository.GetAllAsync(requestModel);
        return mapper.Map(comments);
    }
}