using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.UserStoryComments.Queries;
using Store.Domain.Dtos.UserStoryComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IUserStoryCommentMapperService : IMapper
{
    PagedResult<GetAllUserStoryCommentResponse> Map(PagedResult<GetAllUserStoryCommentResponseDto> model);
    PagedResult<SearchUserStoryCommentResponse> Map(PagedResult<SearchUserStoryCommentResponseDto> model);
    GetAllUserStoryCommentRequestDto Map(GetAllUserStoryCommentRequest model);
    SearchUserStoryCommentRequestDto Map(SearchUserStoryCommentRequest model);
}
