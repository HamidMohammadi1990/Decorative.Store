using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Section>(context), ISectionRepository
{
    public Task<Section?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Section
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Section?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Section
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<GetAllSectionResponseDto>> GetAllAsync(
        GetAllSectionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var sections = Context.Section
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        sections = ApplyTranslationFilters(sections, request.Title, request.Url);

        return await sections
            .Select(x => new GetAllSectionResponseDto
            {
                Id = x.Id,
                SectionTypeId = x.SectionTypeId,
                ParentId = x.ParentId,
                ImageUrl = x.ImageUrl,
                StartDateOnUtc = x.StartDateOnUtc,
                EndDateOnUtc = x.EndDateOnUtc,
                IsActive = x.IsActive,
                AdminDescription = x.AdminDescription,
                SectionTypeName = x.SectionType.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? x.SectionType.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? string.Empty,
                ParentTitle = x.ParentId.HasValue
                    ? x.Parent!.Translations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                        ?? x.Parent!.Translations
                            .Where(t => t.LanguageId == defaultLanguageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()
                    : null,
                Translations = x.Translations
                    .Select(t => new SectionTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Description = t.Description,
                        Url = t.Url,
                    })
                    .ToList(),
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchSectionResponseDto>> SearchAsync(
        SearchSectionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var sections = Context.Section
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .Where(x => x.SectionType.IsActive)
            .Where(x => !x.StartDateOnUtc.HasValue || x.StartDateOnUtc <= DateTime.UtcNow)
            .Where(x => !x.EndDateOnUtc.HasValue || x.EndDateOnUtc >= DateTime.UtcNow)
            .ApplyQueryFilters(request);

        sections = ApplyTranslationFilters(sections, request.Title, request.Url);

        return await sections
            .Select(x => new SearchSectionResponseDto
            {
                Id = x.Id,
                SectionTypeId = x.SectionTypeId,
                ParentId = x.ParentId,
                Title = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Description = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Description)
                        .FirstOrDefault(),
                Url = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Url)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Url)
                        .FirstOrDefault()
                    ?? string.Empty,
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
            })
            .ToPagedAsync(request.Pagination);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }

    private static IQueryable<Section> ApplyTranslationFilters(IQueryable<Section> query, string? title, string? url)
    {
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Translations.Any(t => t.Title.Contains(title)));

        if (!string.IsNullOrWhiteSpace(url))
            query = query.Where(x => x.Translations.Any(t => t.Url.Contains(url)));

        return query;
    }
}
