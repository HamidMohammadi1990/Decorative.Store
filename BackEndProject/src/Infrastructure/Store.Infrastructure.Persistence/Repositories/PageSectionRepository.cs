using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.PageSections;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class PageSectionRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<PageSection>(context), IPageSectionRepository
{
    public async Task<PagedResult<GetAllPageSectionResponseDto>> GetAllAsync(
        GetAllPageSectionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var pageSections = Context.PageSection
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await pageSections
            .AsNoTracking()
            .Select(x => new GetAllPageSectionResponseDto
            {
                Id = x.Id,
                PageId = x.PageId,
                SectionId = x.SectionId,
                Priority = x.Priority,
                AdminDescription = x.AdminDescription,
                PageTitle = x.Page.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.Page.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                PageSlug = x.Page.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.Page.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
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
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<PageSection>> SearchAsync(SearchPageSectionRequestDto request)
    {
        var pageSections = Context.PageSection
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await pageSections
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    private async Task<(int LanguageId, int DefaultLanguageId)> ResolveLanguageIdsAsync(CancellationToken cancellationToken)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        return (languageId, defaultLanguage.Id);
    }
}
