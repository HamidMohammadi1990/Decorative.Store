using Edition.Application.Features.CommentTopics.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CommentTopics;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICommentTopicMapperService : IMapper
{
    GetCommentTopicResponse Map(CommentTopic model);
    GetAllCommentTopicRequestDto Map(GetAllCommentTopicRequest model);
    SearchCommentTopicRequestDto Map(SearchCommentTopicRequest model);
    PagedResult<GetAllCommentTopicResponse> Map(PagedResult<GetAllCommentTopicResponseDto> model);
    PagedResult<SearchCommentTopicResponse> Map(PagedResult<SearchCommentTopicResponseDto> model);
}