using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
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

    public GetPropertyResponse Map(Property model, string title, string? description)
    {
        return new GetPropertyResponse
        {
            Id = model.Id,
            Code = model.Code,
            Title = title,
            ParentId = model.ParentId,
            Priority = model.Priority,
            IsActive = model.IsActive,
            Description = description,
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
                Code = x.Code,
                Priority = x.Priority,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                PropertyType = x.PropertyType,
                PropertyCategoryId = x.PropertyCategoryId,
                PropertyCategoryCode = x.PropertyCategoryCode,
                Translations = x.Translations
                    .Select(t => new PropertyTranslationItemResponse
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Description = t.Description
                    })
                    .ToList()
            })
            .ToList();

        return PagedResult<GetAllPropertyResponse>.Create(items, model);
    }
}
