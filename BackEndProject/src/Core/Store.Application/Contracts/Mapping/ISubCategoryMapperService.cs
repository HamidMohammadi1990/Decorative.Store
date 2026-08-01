using Edition.Application.Features.SubCategories.Queries;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ISubCategoryMapperService : IMapper
{
    GetSubCategoryResponse Map(SubCategory model, string title, string slug);
    GetAllSubCategoryRequestDto Map(GetAllSubCategoryRequest model);
    SearchSubCategoryRequestDto Map(SearchSubCategoryRequest model);
    PagedResult<GetAllSubCategoryResponse> Map(PagedResult<GetAllSubCategoryResponseDto> model);
    PagedResult<SearchSubCategoryResponse> Map(PagedResult<SearchSubCategoryResponseDto> model);
}