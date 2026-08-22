using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class BlogPostCategoryRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<BlogPostCategory>(context), IBlogPostCategoryRepository
{
    public Task<BlogPostCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.BlogPostCategory
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<BlogPostCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.BlogPostCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludeCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.BlogPostCategory.AnyAsync(
            x => x.Code == normalizedCode && (!excludeCategoryId.HasValue || x.Id != excludeCategoryId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.BlogPostCategoryTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeCategoryId.HasValue || x.BlogPostCategoryId != excludeCategoryId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllBlogPostCategoryResponseDto>> GetAllAsync(
        GetAllBlogPostCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var categories = Context.BlogPostCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        categories = ApplyTranslationFilters(categories, request.Title, request.Slug);

        var result = await categories
            .Select(x => new GetAllBlogPostCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive,
                Translations = x.Translations
                    .Select(t => new TranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug,
                    })
                    .ToList(),
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchBlogPostCategoryResponseDto>> SearchAsync(
        SearchBlogPostCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var categories = Context.BlogPostCategory
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        categories = ApplyTranslationFilters(categories, request.Title, request.Slug);

        var result = await categories
            .Select(x => new SearchBlogPostCategoryResponseDto
            {
                Id = x.Id,
                Code = x.Code,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Slug = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                IsActive = x.IsActive,
                PostCount = x.BlogPosts.Count(p => p.IsActive && p.IsPublished),
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private static IQueryable<BlogPostCategory> ApplyTranslationFilters(
        IQueryable<BlogPostCategory> query,
        string? title,
        string? slug)
    {
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Translations.Any(t => t.Title.Contains(title)));

        if (!string.IsNullOrWhiteSpace(slug))
            query = query.Where(x => x.Translations.Any(t => t.Slug == slug));

        return query;
    }
}
