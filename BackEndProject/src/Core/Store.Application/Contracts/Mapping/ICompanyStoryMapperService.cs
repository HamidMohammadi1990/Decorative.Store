using Edition.Application.Features.CompanyStories.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStories;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICompanyStoryMapperService : IMapper
{
    GetCompanyStoryResponse Map(CompanyStory model);
    GetAllCompanyStoryRequestDto Map(GetAllCompanyStoryRequest model);
    SearchCompanyStoryRequestDto Map(SearchCompanyStoryRequest model);
    PagedResult<GetAllCompanyStoryResponse> Map(PagedResult<GetAllCompanyStoryDto> model);
    PagedResult<SearchCompanyStoryResponse> Map(PagedResult<SearchCompanyStoryDto> model);
}
