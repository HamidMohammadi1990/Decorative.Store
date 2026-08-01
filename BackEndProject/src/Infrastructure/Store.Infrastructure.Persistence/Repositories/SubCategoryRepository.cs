using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.SubCategories;
using Store.Domain.Dtos.Localization;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<SubCategory>(context), ISubCategoryRepository
{
    public Task<SubCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.SubCategory
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<SubCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.SubCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludeSubCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.SubCategory.AnyAsync(
            x => x.Code == normalizedCode && (!excludeSubCategoryId.HasValue || x.Id != excludeSubCategoryId.Value),
            cancellationToken);
    }

    public Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeSubCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedTitle = title.Trim();
        var normalizedSlug = slug.Trim();

        return Context.SubCategoryTranslation.AnyAsync(
            x => x.LanguageId == languageId &&
                 (x.Title == normalizedTitle || x.Slug == normalizedSlug) &&
                 (!excludeSubCategoryId.HasValue || x.SubCategoryId != excludeSubCategoryId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllSubCategoryResponseDto>> GetAllAsync(
        GetAllSubCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var subCategorySource = Context.SubCategory
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from subCategory in subCategorySource
            join category in Context.Category on subCategory.CategoryId equals category.Id
            select new { category, subCategory };

        if (!string.IsNullOrWhiteSpace(request.Code))
            query = query.Where(x => x.subCategory.Code.Contains(request.Code));

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.subCategory.CategoryId == request.CategoryId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.subCategory.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.CategoryCode))
            query = query.Where(x => x.category.Code.Contains(request.CategoryCode));

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Title.Contains(request.Title)));

        if (!string.IsNullOrWhiteSpace(request.Slug))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Slug == request.Slug));

        if (!string.IsNullOrWhiteSpace(request.CategoryTitle))
            query = query.Where(x => x.category.Translations.Any(t => t.Title.Contains(request.CategoryTitle)));

        var result = await query
            .Select(x => new GetAllSubCategoryResponseDto
            {
                Id = x.subCategory.Id,
                Code = x.subCategory.Code,
                IsActive = x.subCategory.IsActive,
                CategoryId = x.subCategory.CategoryId,
                CategoryCode = x.category.Code,
                Translations = x.subCategory.Translations
                    .Select(t => new TranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug
                    })
                    .ToList(),
                CategoryTranslations = x.category.Translations
                    .Select(t => new TranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Slug = t.Slug
                    })
                    .ToList()
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchSubCategoryResponseDto>> SearchAsync(
        SearchSubCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (languageId, defaultLanguageId) = await ResolveLanguageIdsAsync(cancellationToken);

        var subCategorySource = Context.SubCategory
            .AsNoTracking()
            .ApplyContentPolicyFilter(request.ContentFilter);

        var query =
            from subCategory in subCategorySource
            join category in Context.Category on subCategory.CategoryId equals category.Id
            where subCategory.IsActive
            select new { category, subCategory };

        if (!string.IsNullOrWhiteSpace(request.Code))
            query = query.Where(x => x.subCategory.Code.Contains(request.Code));

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.subCategory.CategoryId == request.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(request.CategoryCode))
            query = query.Where(x => x.category.Code.Contains(request.CategoryCode));

        if (!string.IsNullOrWhiteSpace(request.Title))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Title.Contains(request.Title)));

        if (!string.IsNullOrWhiteSpace(request.Slug))
            query = query.Where(x => x.subCategory.Translations.Any(t => t.Slug == request.Slug));

        if (!string.IsNullOrWhiteSpace(request.CategoryTitle))
            query = query.Where(x => x.category.Translations.Any(t => t.Title.Contains(request.CategoryTitle)));

        var result = await query
            .Select(x => new SearchSubCategoryResponseDto
            {
                Id = x.subCategory.Id,
                Code = x.subCategory.Code,
                CategoryId = x.subCategory.CategoryId,
                CategoryCode = x.category.Code,
                Title = x.subCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.subCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                Slug = x.subCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.subCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty,
                CategoryTitle = x.category.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.category.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CategorySlug = x.category.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? x.category.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault()
                    ?? string.Empty
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
}
