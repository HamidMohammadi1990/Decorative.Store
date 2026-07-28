using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostRepository
    (EditionDbContext context)
    : Repository<BlogPost>(context), IBlogPostRepository
{
    public async Task<PagedResult<GetAllBlogPostDto>> GetAllAsync(GetAllBlogPostRequestDto request)
    {
        var blogPosts = Context.BlogPost
            .ApplyContentPolicyFilter(request.ContentFilter);

        var posts =
                from blogPost in blogPosts
                join user in Context.User on blogPost.UserId equals user.Id
                join blogPostCategory in Context.BlogPostCategory on blogPost.BlogPostCategoryId equals blogPostCategory.Id
                select new { blogPost, blogPostCategory, user };

        posts = posts.ApplyQueryFilters(request);

        var result = await posts
            .Select(x => new GetAllBlogPostDto
            {
                Id = x.blogPost.Id,
                Slug = x.blogPost.Slug,
                Title = x.blogPost.Title,
                UserId = x.blogPost.UserId,
                Content = x.blogPost.Content,
                IsActive = x.blogPost.IsActive,
                CategoryId = x.blogPost.BlogPostCategoryId,
                SeoKeywords = x.blogPost.SeoKeywords,
                CreatedOnUtc = x.blogPost.CreatedOnUtc,
                IsPublished = x.blogPost.IsPublished,
                PublishedOnUtc = x.blogPost.PublishedOnUtc,
                UpdatedOnUtc = x.blogPost.UpdatedOnUtc,
                UserFirstName = x.user.FirstName!,
                UserLastName = x.user.LastName!,
                CategoryTitle = x.blogPostCategory.Title,
                MetaDescription = x.blogPost.MetaDescription,
                ReadingTimeInMinutes = x.blogPost.ReadingTimeInMinutes
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostDto>> SearchAsync(SearchBlogPostRequestDto request)
    {
        var blogPosts = Context.BlogPost
            .ApplyContentPolicyFilter(request.ContentFilter);

        var posts =
                from blogPost in blogPosts
                join user in Context.User on blogPost.UserId equals user.Id
                join blogPostCategory in Context.BlogPostCategory on blogPost.BlogPostCategoryId equals blogPostCategory.Id
                where blogPost.IsActive && blogPost.IsPublished
                select new { blogPost, blogPostCategory, user };

        posts = posts.ApplyQueryFilters(request);

        var result = await posts
            .Select(x => new SearchBlogPostDto
            {
                Id = x.blogPost.Id,
                Slug = x.blogPost.Slug,
                Title = x.blogPost.Title,
                UserId = x.blogPost.UserId,
                Content = x.blogPost.Content,
                IsActive = x.blogPost.IsActive,
                CategoryId = x.blogPost.BlogPostCategoryId,
                SeoKeywords = x.blogPost.SeoKeywords,
                CreatedOnUtc = x.blogPost.CreatedOnUtc,
                IsPublished = x.blogPost.IsPublished,
                PublishedOnUtc = x.blogPost.PublishedOnUtc,
                UpdatedOnUtc = x.blogPost.UpdatedOnUtc,
                UserFirstName = x.user.FirstName!,
                UserLastName = x.user.LastName!,
                CategoryTitle = x.blogPostCategory.Title,
                MetaDescription = x.blogPost.MetaDescription,
                ReadingTimeInMinutes = x.blogPost.ReadingTimeInMinutes
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}