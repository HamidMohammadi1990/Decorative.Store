using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.UserStoryComments.Queries;

public class SearchUserStoryCommentHandler
    (IUserStoryCommentRepository userStoryCommentRepository, IUserStoryCommentMapperService mapper)
    : IRequestHandler<SearchUserStoryCommentRequest, OperationResult<PagedResult<SearchUserStoryCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchUserStoryCommentResponse>>> Handle(
        SearchUserStoryCommentRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await userStoryCommentRepository.SearchAsync(requestModel);
        return mapper.Map(comments);
    }
}
