using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.CommentTopics.Queries;

public class SearchCommentTopicHandler
    (ICommentTopicRepository commentTopicRepository, ICommentTopicMapperService mapper)
    : IRequestHandler<SearchCommentTopicRequest, OperationResult<PagedResult<SearchCommentTopicResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCommentTopicResponse>>> Handle(SearchCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var commentTopics = await commentTopicRepository.SearchAsync(requestModel);
        var result = mapper.Map(commentTopics);
        return result;
    }
}