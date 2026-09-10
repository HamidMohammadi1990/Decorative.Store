using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class SectionTypeRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<SectionType>(context), ISectionTypeRepository
{
    public Task<SectionType?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.SectionType
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<SectionType?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.SectionType
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string name,
        int? excludeSectionTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return Context.SectionTypeTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 x.Name == normalizedName &&
                 (!excludeSectionTypeId.HasValue || x.SectionTypeId != excludeSectionTypeId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllSectionTypeResponseDto>> GetAllAsync(
        GetAllSectionTypeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var sectionTypes = Context.SectionType
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Name))
            sectionTypes = sectionTypes.Where(x => x.Translations.Any(t => t.Name.Contains(request.Name)));

        return await sectionTypes
            .Select(x => new GetAllSectionTypeResponseDto
            {
                Id = x.Id,
                IsActive = x.IsActive,
                AdminDescription = x.AdminDescription,
                Translations = x.Translations
                    .Select(t => new SectionTypeTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Name = t.Name,
                    })
                    .ToList(),
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchSectionTypeResponseDto>> SearchAsync(
        SearchSectionTypeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var sectionTypes = Context.SectionType
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Name))
            sectionTypes = sectionTypes.Where(x => x.Translations.Any(t => t.Name.Contains(request.Name)));

        return await sectionTypes
            .Select(x => new SearchSectionTypeResponseDto
            {
                Id = x.Id,
                Name = x.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                    ?? string.Empty,
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
