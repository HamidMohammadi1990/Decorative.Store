using Edition.Application.Features.Sections.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ISectionMapperService : IMapper
{
    GetSectionResponse Map(Section model);
    GetAllSectionRequestDto Map(GetAllSectionRequest model);
    SearchSectionRequestDto Map(SearchSectionRequest model);
    PagedResult<GetAllSectionResponse> Map(PagedResult<Section> model);
    PagedResult<SearchSectionResponse> MapToSearch(PagedResult<Section> model);
}
