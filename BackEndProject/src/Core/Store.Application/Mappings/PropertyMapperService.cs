using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Properties.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Properties;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PropertyMapperService : IPropertyMapperService
{
    public GetAllPropertyRequestDto Map(GetAllPropertyRequest model)
    {
        return new GetAllPropertyRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            ParentId = model.ParentId,
            Pagination = model.Pagination,
            PropertyType = model.PropertyType,
            PropertyCategoryId = model.PropertyCategoryId
        }.WithContentPolicy<Property, GetAllPropertyRequestDto>(model);
    }

    public GetPropertyResponse Map(Property model)
    {
        return new GetPropertyResponse
        {
            Id = model.Id,
            Title = model.Title,
            ParentId = model.ParentId,
            Priority = model.Priority,
            IsActive = model.IsActive,
            Description = model.Description,
            PropertyType = model.PropertyType,
            PropertyCategoryId = model.PropertyCategoryId
        };
    }

    public PagedResult<GetAllPropertyResponse> Map(PagedResult<GetAllPropertyDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPropertyResponse
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                Description = x.Description,
                PropertyType = x.PropertyType,
                PropertyCategoryId = x.PropertyCategoryId,
                PropertyCategoryTitle = x.PropertyCategoryTitle
            })
            .ToList();

        return PagedResult<GetAllPropertyResponse>.Create(items, model);
    }
}