using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.BlogPosts.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class BlogPostMapperService : IBlogPostMapperService
{
    public PagedResult<GetAllBlogPostResponse> Map(PagedResult<GetAllBlogPostDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllBlogPostResponse
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                UserId = x.UserId,
                Content = x.Content,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,
                SeoKeywords = x.SeoKeywords,
                IsPublished = x.IsPublished,
                CreatedOnUtc = x.CreatedOnUtc,
                PublishedOnUtc = x.PublishedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CategoryTitle = x.CategoryTitle,
                MetaDescription = x.MetaDescription,
                ReadingTimeInMinutes = x.ReadingTimeInMinutes
            })
            .ToList();

        return PagedResult<GetAllBlogPostResponse>.Create(items, model);
    }

    public PagedResult<SearchBlogPostResponse> Map(PagedResult<SearchBlogPostDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchBlogPostResponse
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                UserId = x.UserId,
                Content = x.Content,
                CategoryId = x.CategoryId,
                SeoKeywords = x.SeoKeywords,
                CreatedOnUtc = x.CreatedOnUtc,
                PublishedOnUtc = x.PublishedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                CategoryTitle = x.CategoryTitle,
                MetaDescription = x.MetaDescription,
                ReadingTimeInMinutes = x.ReadingTimeInMinutes
            })
            .ToList();

        return PagedResult<SearchBlogPostResponse>.Create(items, model);
    }

    public GetBlogPostResponse Map(BlogPost model)
    {
        return new GetBlogPostResponse
        {
            Id = model.Id,
            Slug = model.Slug,
            Title = model.Title,
            UserId = model.UserId,
            Content = model.Content,
            IsActive = model.IsActive,
            IsPublished = model.IsPublished,
            SeoKeywords = model.SeoKeywords,
            UpdatedOnUtc = model.UpdatedOnUtc,
            CreatedOnUtc = model.CreatedOnUtc,
            PublishedOnUtc = model.PublishedOnUtc,
            MetaDescription = model.MetaDescription,
            BlogPostCategoryId = model.BlogPostCategoryId,
            ReadingTimeInMinutes = model.ReadingTimeInMinutes
        };
    }

    public GetAllBlogPostRequestDto Map(GetAllBlogPostRequest model)
    {
        return new GetAllBlogPostRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            UserId = model.UserId,
            IsActive = model.IsActive,
            CategoryId = model.CategoryId,
            Pagination = model.Pagination,
            IsPublished = model.IsPublished,
        }.WithContentPolicy<BlogPost, GetAllBlogPostRequestDto>(model);
    }

    public SearchBlogPostRequestDto Map(SearchBlogPostRequest model)
    {
        return new SearchBlogPostRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            CategoryId = model.CategoryId,
            Pagination = model.Pagination
        }.WithContentPolicy<BlogPost, SearchBlogPostRequestDto>(model);
    }
}