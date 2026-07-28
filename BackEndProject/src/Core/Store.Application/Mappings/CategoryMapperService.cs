using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Categories.Queries;
using Store.Domain.Dtos.Categories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CategoryMapperService : ICategoryMapperService
{
    public List<GetCategoriesWithProductsResponse> Map(List<CategoryWithSubCategoriesDto> categories)
    {
        return
            [.. categories
            .Select(category => new GetCategoriesWithProductsResponse
            {
                Id = category.Id,
                Title = category.Title,
                Slug = category.Slug,
                SubCategories = [.. category.SubCategories.Select(subCategory => new SubCategoryWithProductsResponse
                {
                    Id = subCategory.Id,
                    Title = subCategory.Title,
                    Slug = subCategory.Slug,
                    Products = [.. subCategory.Products.Select(product => new ProductForSubCategoryResponse
                    {
                        Id = product.Id,
                        Title = product.Title,
                        Slug = product.Slug
                    })]
                })]
            })];
    }

    public GetCategoryResponse Map(Category model)
    {
        return new GetCategoryResponse
        {
            Id = model.Id,
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public GetAllCategoryRequestDto Map(GetAllCategoryRequest model)
    {
        return new GetAllCategoryRequestDto
        {
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
            IsActive = model.IsActive,
        }.WithContentPolicy<Category, GetAllCategoryRequestDto>(model);
    }

    public SearchCategoryRequestDto Map(SearchCategoryRequest model)
    {
        return new SearchCategoryRequestDto
        {
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
        }.WithContentPolicy<Category, SearchCategoryRequestDto>(model);
    }

    public PagedResult<GetAllCategoryResponse> Map(PagedResult<GetAllCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCategoryResponse
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllCategoryResponse>.Create(items, model);
    }

    public PagedResult<SearchCategoryResponse> Map(PagedResult<SearchCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCategoryResponse
            {
                Id = x.Id,
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title                
            })
            .ToList();

        return PagedResult<SearchCategoryResponse>.Create(items, model);
    }
}