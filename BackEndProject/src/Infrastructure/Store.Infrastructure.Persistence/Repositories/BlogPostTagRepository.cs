using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostTags;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostTagRepository
    (EditionDbContext context)
    : Repository<BlogPostTag>(context), IBlogPostTagRepository
{
    public async Task<PagedResult<GetAllBlogPostTagDto>> GetAllAsync(GetAllBlogPostTagRequestDto request)
    {
        var blogPostTagSource = Context.BlogPostTag
            .ApplyContentPolicyFilter(request.ContentFilter);

        var tags =
            from blogPostTag in blogPostTagSource
            join blogPost in Context.BlogPost on blogPostTag.BlogPostId equals blogPost.Id
            join tag in Context.Tag on blogPostTag.TagId equals tag.Id
            select new { blogPost, tag, blogPostTag };

        tags = tags.ApplyQueryFilters(request);

        var result = await tags
            .Select(x => new GetAllBlogPostTagDto
            {
                Id = x.blogPostTag.Id,
                TagId = x.blogPostTag.TagId,
                TagTitle = x.tag.Title,
                BlogPostId = x.blogPostTag.BlogPostId,
                BlogPostTitle = x.blogPost.Translations.Select(t => t.Title).FirstOrDefault() ?? string.Empty
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostTagDto>> SearchAsync(SearchBlogPostTagRequestDto request)
    {
        var blogPostTagSource = Context.BlogPostTag
            .ApplyContentPolicyFilter(request.ContentFilter);

        var tags =
            from blogPostTag in blogPostTagSource
            join blogPost in Context.BlogPost on blogPostTag.BlogPostId equals blogPost.Id
            join tag in Context.Tag on blogPostTag.TagId equals tag.Id
            select new { blogPost, tag, blogPostTag };

        tags = tags.ApplyQueryFilters(request);

        var result = await
            tags
            .Select(x => new SearchBlogPostTagDto
            {
                Id = x.blogPostTag.Id,
                TagId = x.blogPostTag.TagId,
                TagTitle = x.tag.Title,
                BlogPostId = x.blogPostTag.BlogPostId,
                BlogPostTitle = x.blogPost.Translations.Select(t => t.Title).FirstOrDefault() ?? string.Empty
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}