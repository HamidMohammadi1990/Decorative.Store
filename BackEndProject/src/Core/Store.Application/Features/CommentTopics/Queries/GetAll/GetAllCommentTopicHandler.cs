using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.CommentTopics.Queries;

public class GetAllCommentTopicHandler
    (ICommentTopicRepository commentTopicRepository, ICommentTopicMapperService mapper)
    : IRequestHandler<GetAllCommentTopicRequest, OperationResult<PagedResult<GetAllCommentTopicResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCommentTopicResponse>>> Handle(GetAllCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var commentTopics = await commentTopicRepository.GetAllAsync(requestModel);
        var result = mapper.Map(commentTopics);
        return result;
    }
}