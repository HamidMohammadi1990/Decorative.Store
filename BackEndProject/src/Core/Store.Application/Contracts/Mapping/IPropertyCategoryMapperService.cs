using Edition.Application.Features.PropertyCategories.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPropertyCategoryMapperService : IMapper
{
    GetPropertyCategoryResponse Map(PropertyCategory model);
    PagedResult<GetAllPropertyCategoryResponse> Map(PagedResult<PropertyCategory> model);
    GetAllPropertyCategoryRequestDto Map(GetAllPropertyCategoryRequest model);
}