using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostCategories.Queries;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostCategoryMapperService : IBlogPostCategoryMapperService
{
    public PagedResult<GetAllBlogPostCategoryResponse> Map(PagedResult<GetAllBlogPostCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostCategoryResponse
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllBlogPostCategoryResponse>.Create(items, model);
    }

    public PagedResult<SearchBlogPostCategoryResponse> Map(PagedResult<SearchBlogPostCategoryResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchBlogPostCategoryResponse
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<SearchBlogPostCategoryResponse>.Create(items, model);
    }

    public GetBlogPostCategoryResponse Map(BlogPostCategory model)
    {
        return new GetBlogPostCategoryResponse
        {
            Id = model.Id,
            Slug = model.Slug,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public GetAllBlogPostCategoryRequestDto Map(GetAllBlogPostCategoryRequest model)
    {
        return new GetAllBlogPostCategoryRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostCategory, GetAllBlogPostCategoryRequestDto>(model);
    }

    public SearchBlogPostCategoryRequestDto Map(SearchBlogPostCategoryRequest model)
    {
        return new SearchBlogPostCategoryRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPostCategory, SearchBlogPostCategoryRequestDto>(model);
    }
}