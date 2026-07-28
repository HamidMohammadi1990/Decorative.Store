using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PropertyItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItems;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PropertyItemMapperService : IPropertyItemMapperService
{
    public GetAllPropertyItemRequestDto Map(GetAllPropertyItemRequest model)
    {
        return new GetAllPropertyItemRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
            PropertyId = model.PropertyId,
            PropertyType = model.PropertyType,
            PropertyTitle = model.PropertyTitle,
            PropertyCategoryId = model.PropertyCategoryId
        }.WithContentPolicy<PropertyItem, GetAllPropertyItemRequestDto>(model);
    }

    public GetPropertyItemResponse Map(PropertyItem model)
    {
        return new GetPropertyItemResponse
        {
            Id = model.Id,
            Title = model.Title,
            Priority = model.Priority,
            IsActive = model.IsActive,
            PropertyId = model.PropertyId
        };
    }

    public PagedResult<GetAllPropertyItemResponse> Map(PagedResult<GetAllPropertyItemDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPropertyItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive,
                PropertyId = x.PropertyId,
                PropertyType = x.PropertyType,
                PropertyTitle = x.PropertyTitle,
                PropertyCategoryId = x.PropertyCategoryId
            })
            .ToList();

        return PagedResult<GetAllPropertyItemResponse>.Create(items, model);
    }
}