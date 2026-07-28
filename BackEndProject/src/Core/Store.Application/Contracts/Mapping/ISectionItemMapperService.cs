using Edition.Application.Features.SectionItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.SectionItems;

namespace Edition.Application.Contracts.Mapping;

public interface ISectionItemMapperService : IMapper
{
    GetSectionItemResponse Map(SectionItem model);
    GetAllSectionItemRequestDto Map(GetAllSectionItemRequest model);
    SearchSectionItemRequestDto Map(SearchSectionItemRequest model);
    PagedResult<GetAllSectionItemResponse> Map(PagedResult<SectionItem> model);
    PagedResult<SearchSectionItemResponse> MapToSearch(PagedResult<SectionItem> model);
}
