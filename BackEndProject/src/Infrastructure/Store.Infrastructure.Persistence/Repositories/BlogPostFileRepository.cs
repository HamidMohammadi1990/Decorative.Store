using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.BlogPostFiles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostFileRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<BlogPostFile>(context), IBlogPostFileRepository
{
    public Task<BlogPostFile?> GetWithTranslationsAsNoTrackingAsync(
        int id,
        CancellationToken cancellationToken = default)
        => Context.BlogPostFile
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<BlogPostFile?> GetWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.BlogPostFile
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task ClearMainFlagsAsync(int blogPostId, CancellationToken cancellationToken = default)
    {
        var mainFiles = await Context.BlogPostFile
            .Where(x => x.BlogPostId == blogPostId && x.IsMain)
            .ToListAsync(cancellationToken);

        foreach (var file in mainFiles)
            file.SetMain(false);
    }

    public async Task<PagedResult<GetAllBlogPostFileResponseDto>> GetAllAsync(
        GetAllBlogPostFileRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var blogPostFileSource = Context.BlogPostFile
            .ApplyContentPolicyFilter(request.ContentFilter);

        var blogPostFiles =
            from blogPostFile in blogPostFileSource
            join blogPost in Context.BlogPost on blogPostFile.BlogPostId equals blogPost.Id
            select new { blogPostFile, blogPost };

        if (request.BlogPostId.HasValue)
            blogPostFiles = blogPostFiles.Where(x => x.blogPostFile.BlogPostId == request.BlogPostId.Value);

        if (request.IsActive.HasValue)
            blogPostFiles = blogPostFiles.Where(x => x.blogPostFile.IsActive == request.IsActive.Value);

        if (request.IsMain.HasValue)
            blogPostFiles = blogPostFiles.Where(x => x.blogPostFile.IsMain == request.IsMain.Value);

        if (!string.IsNullOrWhiteSpace(request.Title))
            blogPostFiles = blogPostFiles.Where(x => x.blogPostFile.Translations.Any(t => t.Title.Contains(request.Title)));

        var result = await blogPostFiles
            .Select(x => new GetAllBlogPostFileResponseDto
            {
                Id = x.blogPostFile.Id,
                IsMain = x.blogPostFile.IsMain,
                IsActive = x.blogPostFile.IsActive,
                FileName = x.blogPostFile.FileName,
                BlogPostId = x.blogPostFile.BlogPostId,
                Title = x.blogPostFile.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPostFile.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                BlogPostTitle = x.blogPost.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.blogPost.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<BlogPostImageDto>> GetActiveImagesByBlogPostIdAsync(
        int blogPostId,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        return await Context.BlogPostFile
            .AsNoTracking()
            .Where(x => x.BlogPostId == blogPostId && x.IsActive)
            .OrderByDescending(x => x.IsMain)
            .ThenBy(x => x.Id)
            .Select(x => new BlogPostImageDto
            {
                Id = x.Id,
                FileName = x.FileName,
                IsMain = x.IsMain,
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
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<int, string>> GetCoverFileNamesByBlogPostIdsAsync(
        IReadOnlyCollection<int> blogPostIds,
        CancellationToken cancellationToken = default)
    {
        if (blogPostIds.Count == 0)
            return new Dictionary<int, string>();

        var files = await Context.BlogPostFile
            .AsNoTracking()
            .Where(x => blogPostIds.Contains(x.BlogPostId) && x.IsActive)
            .Select(x => new { x.BlogPostId, x.FileName, x.IsMain, x.Id })
            .ToListAsync(cancellationToken);

        return files
            .GroupBy(x => x.BlogPostId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderByDescending(x => x.IsMain)
                    .ThenBy(x => x.Id)
                    .First()
                    .FileName);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(
        CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
