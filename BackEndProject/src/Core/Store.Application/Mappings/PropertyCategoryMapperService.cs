using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
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
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<PropertyCategory, GetAllPropertyCategoryRequestDto>(model);
    }

    public GetPropertyCategoryResponse Map(PropertyCategory model, string title)
    {
        return new GetPropertyCategoryResponse
        {
            Id = model.Id,
            Code = model.Code,
            Title = title,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllPropertyCategoryResponse> Map(PagedResult<GetAllPropertyCategoryDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPropertyCategoryResponse
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive,
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

        return PagedResult<GetAllPropertyCategoryResponse>.Create(items, model);
    }
}
