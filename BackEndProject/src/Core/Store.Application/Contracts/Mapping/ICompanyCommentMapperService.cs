using Edition.Application.Features.CompanyComments.Queries;
using Store.Domain.Dtos.CompanyComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyCommentMapperService : IMapper
{
    GetCompanyCommentResponse Map(CompanyComment model);
    GetUserCompanyCommentRequestDto Map(GetAllCompanyCommentRequest model);
    SearchCompanyCommentRequestDto Map(SearchCompanyCommentRequest model);
    PagedResult<GetAllCompanyCommentResponse> Map(PagedResult<GetAllCompanyCommentResponseDto> model);
}