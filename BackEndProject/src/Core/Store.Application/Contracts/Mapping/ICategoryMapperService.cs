using Edition.Application.Features.Categories.Queries;
using Store.Domain.Dtos.Categories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ICategoryMapperService : IMapper
{
    GetCategoryResponse Map(Category model);
    GetAllCategoryRequestDto Map(GetAllCategoryRequest model);
    SearchCategoryRequestDto Map(SearchCategoryRequest model);
    PagedResult<GetAllCategoryResponse> Map(PagedResult<GetAllCategoryResponseDto> model);
    PagedResult<SearchCategoryResponse> Map(PagedResult<SearchCategoryResponseDto> model);
    List<GetCategoriesWithProductsResponse> Map(List<CategoryWithSubCategoriesDto> categories);
}