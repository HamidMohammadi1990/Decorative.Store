using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPostCategories.Queries;
using Edition.Application.Features.Localization;
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
                Code = x.Code,
                IsActive = x.IsActive,
                Translations = x.Translations
                    .Select(t => new TranslationItemResponse
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug,
                    })
                    .ToList(),
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
                Code = x.Code,
                Slug = x.Slug,
                Title = x.Title,
                IsActive = x.IsActive,
                PostCount = x.PostCount,
            })
            .ToList();

        return PagedResult<SearchBlogPostCategoryResponse>.Create(items, model);
    }

    public GetBlogPostCategoryResponse Map(BlogPostCategory model, string title, string slug)
    {
        return new GetBlogPostCategoryResponse
        {
            Id = model.Id,
            Code = model.Code,
            Slug = slug,
            Title = title,
            IsActive = model.IsActive,
        };
    }

    public GetAllBlogPostCategoryRequestDto Map(GetAllBlogPostCategoryRequest model)
    {
        return new GetAllBlogPostCategoryRequestDto
        {
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<BlogPostCategory, GetAllBlogPostCategoryRequestDto>(model);
    }

    public SearchBlogPostCategoryRequestDto Map(SearchBlogPostCategoryRequest model)
    {
        return new SearchBlogPostCategoryRequestDto
        {
            Code = model.Code,
            Slug = model.Slug,
            Title = model.Title,
            Pagination = model.Pagination,
        }.WithContentPolicy<BlogPostCategory, SearchBlogPostCategoryRequestDto>(model);
    }
}
