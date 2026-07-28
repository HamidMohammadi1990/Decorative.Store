using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PropertyCategories.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PropertyCategoryMapperService : IPropertyCategoryMapperService
{
    public GetAllPropertyCategoryRequestDto Map(GetAllPropertyCategoryRequest model)
    {
        return new GetAllPropertyCategoryRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive
        }.WithContentPolicy<PropertyCategory, GetAllPropertyCategoryRequestDto>(model);
    }

    public GetPropertyCategoryResponse Map(PropertyCategory model)
    {
        return new GetPropertyCategoryResponse
        {
            Id = model.Id,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllPropertyCategoryResponse> Map(PagedResult<PropertyCategory> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPropertyCategoryResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllPropertyCategoryResponse>.Create(items, model);
    }
}