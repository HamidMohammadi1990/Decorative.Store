using Edition.Application.Features.PageSections.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PageSections;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPageSectionMapperService : IMapper
{
    GetPageSectionResponse Map(PageSection model);
    GetAllPageSectionRequestDto Map(GetAllPageSectionRequest model);
    SearchPageSectionRequestDto Map(SearchPageSectionRequest model);
    PagedResult<GetAllPageSectionResponse> Map(PagedResult<PageSection> model);
    PagedResult<SearchPageSectionResponse> MapToSearch(PagedResult<PageSection> model);
}
