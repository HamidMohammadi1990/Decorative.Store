using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostLikes;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostLikeRepository
    (EditionDbContext context)
    : Repository<BlogPostLike>(context), IBlogPostLikeRepository
{
    public async Task<PagedResult<BlogPostLikeDto>> GetAllAsync(GetAllBlogPostLikeRequestDto request)
    {
        var blogPostLikeSource = Context.BlogPostLike
            .ApplyContentPolicyFilter(request.ContentFilter);

        var likes =
             from blogPostLike in blogPostLikeSource
             join blogPost in Context.BlogPost on blogPostLike.BlogPostId equals blogPost.Id
             join user in Context.User on blogPostLike.UserId equals user.Id into joinUser
             from user in joinUser.DefaultIfEmpty()
             select new { blogPostLike, user, blogPost };

        likes = likes.ApplyQueryFilters(request);

        var result = await
            likes
            .Select(x => new BlogPostLikeDto
            {
                Id = x.blogPostLike.Id,
                UserName = x.user.UserName,
                ClientIP = x.blogPostLike.ClientIP,
                BlogPostId = x.blogPostLike.BlogPostId,
                CreatedOnUtc = x.blogPostLike.CreatedOnUtc,
                BlogPostTitle = x.blogPost.Translations.Select(t => t.Title).FirstOrDefault() ?? string.Empty,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}