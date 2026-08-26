using Edition.Application.Features.SectionItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionItems;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ISectionItemMapperService : IMapper
{
    GetSectionItemResponse Map(SectionItem model, string title, string? description, string? url);
    GetAllSectionItemRequestDto Map(GetAllSectionItemRequest model);
    SearchSectionItemRequestDto Map(SearchSectionItemRequest model);
    PagedResult<GetAllSectionItemResponse> Map(PagedResult<GetAllSectionItemResponseDto> model);
    PagedResult<SearchSectionItemResponse> MapToSearch(PagedResult<SearchSectionItemResponseDto> model);
}
