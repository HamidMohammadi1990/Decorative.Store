using Edition.Application.Features.SectionTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ISectionTypeMapperService : IMapper
{
    GetSectionTypeResponse Map(SectionType model);
    GetAllSectionTypeRequestDto Map(GetAllSectionTypeRequest model);
    SearchSectionTypeRequestDto Map(SearchSectionTypeRequest model);
    PagedResult<GetAllSectionTypeResponse> Map(PagedResult<SectionType> model);
    PagedResult<SearchSectionTypeResponse> MapToSearch(PagedResult<SectionType> model);
}
