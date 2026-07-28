using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CommentTopics.Queries;

public class GetCommentTopicHandler
    (ICommentTopicRepository commentTopicRepository, ICommentTopicMapperService mapper)
    : IRequestHandler<GetCommentTopicRequest, OperationResult<GetCommentTopicResponse>>
{
    public async Task<OperationResult<GetCommentTopicResponse>> Handle(GetCommentTopicRequest request, CancellationToken cancellationToken)
    {
        var commentTopic = await commentTopicRepository.GetAsNoTrackingAsync(request.Id);
        if (commentTopic is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(commentTopic);
        return result;
    }
}