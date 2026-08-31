using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.UserStoryComments.Queries;

public class GetAllUserStoryCommentHandler
    (IUserStoryCommentRepository userStoryCommentRepository, IUserStoryCommentMapperService mapper)
    : IRequestHandler<GetAllUserStoryCommentRequest, OperationResult<PagedResult<GetAllUserStoryCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllUserStoryCommentResponse>>> Handle(
        GetAllUserStoryCommentRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await userStoryCommentRepository.GetAllAsync(requestModel);
        return mapper.Map(comments);
    }
}
