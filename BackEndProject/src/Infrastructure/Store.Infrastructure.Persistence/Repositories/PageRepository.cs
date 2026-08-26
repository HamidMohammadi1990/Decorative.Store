using System.Linq.Expressions;
using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Repositories;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class PageRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Page>(context), IPageRepository
{
    public Task<Page?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Page
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Page?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Page
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludePageId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.PageTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludePageId.HasValue || x.PageId != excludePageId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllPageResponseDto>> GetAllAsync(
        GetAllPageRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var pages = Context.Page
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        pages = ApplyTranslationFilters(pages, request.Title, request.Slug);

        return await pages
            .Select(x => new GetAllPageResponseDto
            {
                Id = x.Id,
                Type = x.Type,
                IsActive = x.IsActive,
                Translations = x.Translations
                    .Select(t => new PageTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug,
                        MetaTitle = t.MetaTitle,
                        MetaDescription = t.MetaDescription,
                    })
                    .ToList(),
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchPageResponseDto>> SearchAsync(
        SearchPageRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var pages = Context.Page
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        pages = ApplyTranslationFilters(pages, request.Title, request.Slug);

        return await pages
            .Select(x => new SearchPageResponseDto
            {
                Id = x.Id,
                Type = x.Type,
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
                MetaTitle = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.MetaTitle)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.MetaTitle)
                        .FirstOrDefault(),
                MetaDescription = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.MetaDescription)
                        .FirstOrDefault(),
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<Page?> GetActiveBySlugWithContentAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalizedSlug = slug.Trim();

        return await Context.Page
            .AsNoTracking()
            .Include(p => p.Translations)
            .Include(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                    .ThenInclude(s => s.SectionType)
                        .ThenInclude(st => st.Translations)
            .Include(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                    .ThenInclude(s => s.Translations)
            .Include(p => p.PageSections)
                .ThenInclude(ps => ps.Section)
                    .ThenInclude(s => s.SectionItems)
                        .ThenInclude(i => i.Translations)
            .Where(p => p.IsActive)
            .Where(p => p.Translations.Any(t => t.Slug == normalizedSlug))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> HasPageSectionsAsync(int pageId, CancellationToken cancellationToken = default)
        => Context.PageSection.AnyAsync(x => x.PageId == pageId, cancellationToken);

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private static IQueryable<Page> ApplyTranslationFilters(IQueryable<Page> query, string? title, string? slug)
    {
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Translations.Any(t => t.Title.Contains(title)));

        if (!string.IsNullOrWhiteSpace(slug))
            query = query.Where(x => x.Translations.Any(t => t.Slug.Contains(slug)));

        return query;
    }
}
