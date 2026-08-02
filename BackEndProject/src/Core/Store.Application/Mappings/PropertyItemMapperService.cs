using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
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

    public GetPropertyItemResponse Map(PropertyItem model, string title)
    {
        return new GetPropertyItemResponse
        {
            Id = model.Id,
            Code = model.Code,
            Title = title,
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
                Code = x.Code,
                Priority = x.Priority,
                IsActive = x.IsActive,
                PropertyId = x.PropertyId,
                PropertyCode = x.PropertyCode,
                PropertyType = x.PropertyType,
                PropertyCategoryId = x.PropertyCategoryId,
                Translations = x.Translations
                    .Select(t => new TranslationItemResponse
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = string.Empty
                    })
                    .ToList()
            })
            .ToList();

        return PagedResult<GetAllPropertyItemResponse>.Create(items, model);
    }
}
