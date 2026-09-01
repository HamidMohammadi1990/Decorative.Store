using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionItems;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionItemRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<SectionItem>(context), ISectionItemRepository
{
    public Task<SectionItem?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.SectionItem
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<SectionItem?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.SectionItem
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<GetAllSectionItemResponseDto>> GetAllAsync(
        GetAllSectionItemRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var sectionItems = Context.SectionItem
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Title))
            sectionItems = sectionItems.Where(x => x.Translations.Any(t => t.Title.Contains(request.Title)));

        return await sectionItems
            .Select(x => new GetAllSectionItemResponseDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
                Priority = x.Priority,
                Icon = x.Icon,
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
                SectionTitle = x.Section.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Section.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                SectionTypeName = x.Section.SectionType.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? x.Section.SectionType.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? string.Empty,
                Translations = x.Translations
                    .Select(t => new SectionItemTranslationItemDto
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

    public async Task<PagedResult<SearchSectionItemResponseDto>> SearchAsync(
        SearchSectionItemRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var sectionItems = Context.SectionItem
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Title))
            sectionItems = sectionItems.Where(x => x.Translations.Any(t => t.Title.Contains(request.Title)));

        return await sectionItems
            .Select(x => new SearchSectionItemResponseDto
            {
                Id = x.Id,
                SectionId = x.SectionId,
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
                        .FirstOrDefault(),
                Priority = x.Priority,
                Icon = x.Icon,
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
}
