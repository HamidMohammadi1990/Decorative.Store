using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostComments;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<BlogPost>(context), IBlogPostRepository
{
    public Task<BlogPost?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.BlogPost
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<BlogPost?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.BlogPost
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludeBlogPostId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.BlogPost.AnyAsync(
            x => x.Code == normalizedCode && (!excludeBlogPostId.HasValue || x.Id != excludeBlogPostId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeBlogPostId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.BlogPostTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeBlogPostId.HasValue || x.BlogPostId != excludeBlogPostId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllBlogPostDto>> GetAllAsync(GetAllBlogPostRequestDto request)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        var blogPosts = Context.BlogPost
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter);

        blogPosts = ApplyTranslationFilters(blogPosts, request.Title, request.Slug);

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
                Slug = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserId = x.blogPost.UserId,
                Content = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Content)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Content)
                        .FirstOrDefault()
                    ?? string.Empty,
                IsActive = x.blogPost.IsActive,
                CategoryId = x.blogPost.BlogPostCategoryId,
                SeoKeywords = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? string.Empty,
                CreatedOnUtc = x.blogPost.CreatedOnUtc,
                IsPublished = x.blogPost.IsPublished,
                PublishedOnUtc = x.blogPost.PublishedOnUtc,
                UpdatedOnUtc = x.blogPost.UpdatedOnUtc,
                UserFirstName = x.user.FirstName!,
                UserLastName = x.user.LastName!,
                CategoryTitle = x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                MetaDescription = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? string.Empty,
                ReadingTimeInMinutes = x.blogPost.ReadingTimeInMinutes,
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostDto>> SearchAsync(SearchBlogPostRequestDto request)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        var blogPosts = Context.BlogPost
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive && x.IsPublished);

        blogPosts = ApplyTranslationFilters(blogPosts, request.Title, request.Slug);

        var posts =
            from blogPost in blogPosts
            join user in Context.User on blogPost.UserId equals user.Id
            join blogPostCategory in Context.BlogPostCategory on blogPost.BlogPostCategoryId equals blogPostCategory.Id
            select new { blogPost, blogPostCategory, user };

        posts = posts.ApplyQueryFilters(request);

        var result = await posts
            .Select(x => new SearchBlogPostDto
            {
                Id = x.blogPost.Id,
                Slug = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserId = x.blogPost.UserId,
                Content = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Content)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Content)
                        .FirstOrDefault()
                    ?? string.Empty,
                IsActive = x.blogPost.IsActive,
                CategoryId = x.blogPost.BlogPostCategoryId,
                SeoKeywords = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? string.Empty,
                CreatedOnUtc = x.blogPost.CreatedOnUtc,
                IsPublished = x.blogPost.IsPublished,
                PublishedOnUtc = x.blogPost.PublishedOnUtc,
                UpdatedOnUtc = x.blogPost.UpdatedOnUtc,
                UserFirstName = x.user.FirstName!,
                UserLastName = x.user.LastName!,
                CategoryTitle = x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                MetaDescription = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? string.Empty,
                ReadingTimeInMinutes = x.blogPost.ReadingTimeInMinutes,
                IsFeatured = x.blogPost.IsFeatured,
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<BlogPostDetailDto?> GetDetailBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim();
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync();

        var blogPost = await Context.BlogPost
            .AsNoTracking()
            .Include(x => x.Translations)
            .Where(x => x.IsActive && x.IsPublished)
            .Where(x => x.Translations.Any(t => t.Slug == normalizedSlug))
            .FirstOrDefaultAsync(cancellationToken);

        if (blogPost is null)
            return null;

        var user = await Context.User
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == blogPost.UserId, cancellationToken);

        if (user is null)
            return null;

        var blogPostCategory = await Context.BlogPostCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == blogPost.BlogPostCategoryId, cancellationToken);

        if (blogPostCategory is null)
            return null;

        var post = MapSearchBlogPostDto(blogPost, blogPostCategory, user, languageId, defaultLanguageId);
        if (string.IsNullOrWhiteSpace(post.Slug))
            post = post with { Slug = normalizedSlug };

        var categorySlug = ResolveCategorySlug(blogPostCategory, languageId, defaultLanguageId);
        var blogPostId = blogPost.Id;

        var likeCount = await Context.BlogPostLike
            .AsNoTracking()
            .CountAsync(x => x.BlogPostId == blogPostId, cancellationToken);

        var tagTitles = await (
                from blogPostTag in Context.BlogPostTag.AsNoTracking()
                    .Where(x => x.BlogPostId == blogPostId)
                join tag in Context.Tag.AsNoTracking() on blogPostTag.TagId equals tag.Id
                orderby tag.Title
                select tag.Title)
            .ToListAsync(cancellationToken);

        var comments = await (
                from blogPostComment in Context.BlogPostComment.AsNoTracking()
                    .Where(x => x.BlogPostId == blogPostId && x.IsApproved)
                join createdByUser in Context.User on blogPostComment.CreatedByUserId equals createdByUser.Id
                orderby blogPostComment.CreatedOnUtc descending
                select new SearchBlogPostCommentResponseDto
                {
                    Id = blogPostComment.Id,
                    Content = blogPostComment.Content,
                    ParentId = blogPostComment.ParentId,
                    BlogPostId = blogPostComment.BlogPostId,
                    CreatedOnUtc = blogPostComment.CreatedOnUtc,
                    ApprovedOnUtc = blogPostComment.ApprovedOnUtc,
                    CreatedByUserId = blogPostComment.CreatedByUserId,
                    CreatedByUserFirstName = createdByUser.FirstName,
                    CreatedByUserLastName = createdByUser.LastName,
                })
            .Take(50)
            .ToListAsync(cancellationToken);

        var relatedPosts = await (
                from relatedBlogPost in Context.BlogPost.AsNoTracking()
                    .Where(x => x.IsActive && x.IsPublished)
                    .Where(x => x.BlogPostCategoryId == blogPost.BlogPostCategoryId)
                    .Where(x => x.Id != blogPostId)
                join relatedBlogPostCategory in Context.BlogPostCategory
                    on relatedBlogPost.BlogPostCategoryId equals relatedBlogPostCategory.Id
                orderby relatedBlogPost.PublishedOnUtc descending, relatedBlogPost.CreatedOnUtc descending
                select new { blogPost = relatedBlogPost, blogPostCategory = relatedBlogPostCategory })
            .Take(4)
            .Select(x => new BlogPostDetailRelatedDto
            {
                Id = x.blogPost.Id,
                Slug = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CategoryId = x.blogPost.BlogPostCategoryId,
                CategoryTitle = x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CategorySlug = x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.blogPostCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                MetaDescription = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? string.Empty,
                SeoKeywords = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.SeoKeywords)
                        .FirstOrDefault()
                    ?? string.Empty,
                UserId = x.blogPost.UserId,
                ReadingTimeInMinutes = x.blogPost.ReadingTimeInMinutes,
                CreatedOnUtc = x.blogPost.CreatedOnUtc,
                PublishedOnUtc = x.blogPost.PublishedOnUtc,
                UpdatedOnUtc = x.blogPost.UpdatedOnUtc,
            })
            .ToListAsync(cancellationToken);

        var categoryLabels = await Context.BlogPostCategory
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => new
            {
                Slug = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
            })
            .Where(x => x.Slug != string.Empty)
            .ToDictionaryAsync(x => x.Slug, x => x.Title, cancellationToken);

        return new BlogPostDetailDto
        {
            Post = post,
            CategorySlug = categorySlug,
            LikeCount = likeCount,
            TagTitles = tagTitles,
            Comments = comments,
            RelatedPosts = relatedPosts,
            CategoryLabels = categoryLabels,
        };
    }

    private static SearchBlogPostDto MapSearchBlogPostDto(
        BlogPost blogPost,
        BlogPostCategory blogPostCategory,
        User user,
        int languageId,
        int defaultLanguageId)
    {
        return new SearchBlogPostDto
        {
            Id = blogPost.Id,
            Slug = blogPost.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.Slug)
                    .FirstOrDefault()
                ?? blogPost.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.Slug)
                    .FirstOrDefault()
                ?? string.Empty,
            Title = blogPost.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                ?? blogPost.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                ?? string.Empty,
            UserId = blogPost.UserId,
            Content = blogPost.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.Content)
                    .FirstOrDefault()
                ?? blogPost.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.Content)
                    .FirstOrDefault()
                ?? string.Empty,
            IsActive = blogPost.IsActive,
            CategoryId = blogPost.BlogPostCategoryId,
            SeoKeywords = blogPost.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.SeoKeywords)
                    .FirstOrDefault()
                ?? blogPost.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.SeoKeywords)
                    .FirstOrDefault()
                ?? string.Empty,
            CreatedOnUtc = blogPost.CreatedOnUtc,
            IsPublished = blogPost.IsPublished,
            PublishedOnUtc = blogPost.PublishedOnUtc,
            UpdatedOnUtc = blogPost.UpdatedOnUtc,
            UserFirstName = user.FirstName!,
            UserLastName = user.LastName!,
            CategoryTitle = blogPostCategory.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                ?? blogPostCategory.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                ?? string.Empty,
            MetaDescription = blogPost.Translations
                    .Where(t => t.LanguageId == languageId)
                    .Select(t => t.MetaDescription)
                    .FirstOrDefault()
                ?? blogPost.Translations
                    .Where(t => t.LanguageId == defaultLanguageId)
                    .Select(t => t.MetaDescription)
                    .FirstOrDefault()
                ?? string.Empty,
            ReadingTimeInMinutes = blogPost.ReadingTimeInMinutes,
            IsFeatured = blogPost.IsFeatured,
        };
    }

    private static string ResolveCategorySlug(
        BlogPostCategory blogPostCategory,
        int languageId,
        int defaultLanguageId)
    {
        return blogPostCategory.Translations
                .Where(t => t.LanguageId == languageId)
                .Select(t => t.Slug)
                .FirstOrDefault()
            ?? blogPostCategory.Translations
                .Where(t => t.LanguageId == defaultLanguageId)
                .Select(t => t.Slug)
                .FirstOrDefault()
            ?? string.Empty;
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync()
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync();
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private static IQueryable<BlogPost> ApplyTranslationFilters(
        IQueryable<BlogPost> query,
        string? title,
        string? slug)
    {
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Translations.Any(t => t.Title.Contains(title)));

        if (!string.IsNullOrWhiteSpace(slug))
            query = query.Where(x => x.Translations.Any(t => t.Slug.Contains(slug)));

        return query;
    }
}
