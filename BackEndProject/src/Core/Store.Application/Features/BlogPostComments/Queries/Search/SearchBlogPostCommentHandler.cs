using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class SearchBlogPostCommentHandler
    (IBlogPostCommentRepository blogPostCommentRepository, IBlogPostCommentMapperService mapper)
    : IRequestHandler<SearchBlogPostCommentRequest, OperationResult<PagedResult<SearchBlogPostCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchBlogPostCommentResponse>>> Handle(SearchBlogPostCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await blogPostCommentRepository.SearchAsync(requestModel);
        return mapper.Map(comments);
    }
}