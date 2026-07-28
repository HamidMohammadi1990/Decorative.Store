using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.SubCategories.Queries;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class SubCategoryMapperService : ISubCategoryMapperService
{
    public GetAllSubCategoryRequestDto Map(GetAllSubCategoryRequest model)
    {
        return new GetAllSubCategoryRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Code = model.Code,
            IsActive = model.IsActive,
            CategoryId = model.CategoryId,
            Pagination = model.Pagination,
            CategoryTitle = model.CategoryTitle,
            CategoryCode = model.CategoryCode,
        }.WithContentPolicy<SubCategory, GetAllSubCategoryRequestDto>(model);
    }

    public SearchSubCategoryRequestDto Map(SearchSubCategoryRequest model)
    {
        return new SearchSubCategoryRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Code = model.Code,
            CategoryId = model.CategoryId,
            Pagination = model.Pagination,
            CategoryTitle = model.CategoryTitle,
            CategoryCode = model.CategoryCode,
        }.WithContentPolicy<SubCategory, SearchSubCategoryRequestDto>(model);
    }

    public PagedResult<GetAllSubCategoryResponse> Map(PagedResult<GetAllSubCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllSubCategoryResponse
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,
                CategorySlug = x.CategorySlug,
                CategoryCode = x.CategoryCode,
                CategoryTitle = x.CategoryTitle
            })
            .ToList();

        return PagedResult<GetAllSubCategoryResponse>.Create(items, model);
    }

    public PagedResult<SearchSubCategoryResponse> Map(PagedResult<SearchSubCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchSubCategoryResponse
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title,
                CategoryId = x.CategoryId,
                CategorySlug = x.CategorySlug,
                CategoryCode = x.CategoryCode,
                CategoryTitle = x.CategoryTitle
            })
            .ToList();

        return PagedResult<SearchSubCategoryResponse>.Create(items, model);
    }

    public GetSubCategoryResponse Map(SubCategory model)
    {
        return new GetSubCategoryResponse
        {
            Id = model.Id,
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
            IsActive = !model.IsActive,
            CategoryId = model.CategoryId
        };
    }
}