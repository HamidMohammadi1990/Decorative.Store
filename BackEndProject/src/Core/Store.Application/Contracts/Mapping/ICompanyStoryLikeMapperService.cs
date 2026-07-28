using Edition.Application.Features.CompanyStoryLikes.Queries;
using Store.Domain.Dtos.CompanyStoryLikes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyStoryLikeMapperService : IMapper
{
    GetCompanyStoryLikeResponse Map(CompanyStoryLike model);
    GetAllCompanyStoryLikeRequestDto Map(GetAllCompanyStoryLikeRequest model);
    PagedResult<GetAllCompanyStoryLikeResponse> Map(PagedResult<CompanyStoryLikeDto> model);
}
