using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Dtos.Localization;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyCategoryRepository
    (EditionDbContext context)
    : Repository<PropertyCategory>(context), IPropertyCategoryRepository
{
    public Task<PropertyCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.PropertyCategory
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<PropertyCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.PropertyCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(string code, int? excludePropertyCategoryId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.PropertyCategory.AnyAsync(
            x => x.Code == normalizedCode &&
                 (!excludePropertyCategoryId.HasValue || x.Id != excludePropertyCategoryId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllPropertyCategoryDto>> GetAllAsync(GetAllPropertyCategoryRequestDto request, CancellationToken cancellationToken = default)
    {
        var categories = Context.PropertyCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Title))
            categories = categories.Where(x => x.Translations.Any(t => t.Title.Contains(request.Title)));

        var result = await categories
            .Select(x => new GetAllPropertyCategoryDto
            {
                Id = x.Id,
                Code = x.Code,
                IsActive = x.IsActive,
                Translations = x.Translations
                    .Select(t => new PropertyItemTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title
                    })
                    .ToList()
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
