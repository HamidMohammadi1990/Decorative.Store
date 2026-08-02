using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
using Edition.Application.Features.PropertyCategories.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPropertyCategoryMapperService : IMapper
{
    GetPropertyCategoryResponse Map(PropertyCategory model, string title);
    PagedResult<GetAllPropertyCategoryResponse> Map(PagedResult<GetAllPropertyCategoryDto> model);
    GetAllPropertyCategoryRequestDto Map(GetAllPropertyCategoryRequest model);
}
