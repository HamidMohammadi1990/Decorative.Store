using Edition.Application.Features.CompanyStoryComments.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStoryComments;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyStoryCommentMapperService : IMapper
{
    GetCompanyStoryCommentResponse Map(CompanyStoryComment model);
    GetAllCompanyStoryCommentRequestDto Map(GetAllCompanyStoryCommentRequest model);
    SearchCompanyStoryCommentRequestDto Map(SearchCompanyStoryCommentRequest model);
    PagedResult<GetAllCompanyStoryCommentResponse> Map(PagedResult<GetAllCompanyStoryCommentResponseDto> model);
    PagedResult<SearchCompanyStoryCommentResponse> Map(PagedResult<SearchCompanyStoryCommentResponseDto> model);
}
