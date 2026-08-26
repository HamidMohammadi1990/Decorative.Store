using Edition.Application.Features.Pages.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPageMapperService : IMapper
{
    GetPageResponse Map(
        Page model,
        string title,
        string slug,
        string? metaTitle,
        string? metaDescription);
    GetAllPageRequestDto Map(GetAllPageRequest model);
    SearchPageRequestDto Map(SearchPageRequest model);
    PagedResult<GetAllPageResponse> Map(PagedResult<GetAllPageResponseDto> model);
    PagedResult<SearchPageResponse> MapToSearch(PagedResult<SearchPageResponseDto> model);
}
