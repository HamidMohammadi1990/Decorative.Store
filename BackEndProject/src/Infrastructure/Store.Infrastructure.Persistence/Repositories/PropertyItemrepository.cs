using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItems;
using Store.Domain.Dtos.Localization;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyItemRepository
    (EditionDbContext context)
    : Repository<PropertyItem>(context), IPropertyItemRepository
{
    public Task<PropertyItem?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.PropertyItem
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<PropertyItem?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.PropertyItem
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(
        string code,
        int propertyId,
        int? excludePropertyItemId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.PropertyItem.AnyAsync(
            x => x.Code == normalizedCode &&
                 x.PropertyId == propertyId &&
                 (!excludePropertyItemId.HasValue || x.Id != excludePropertyItemId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllPropertyItemDto>> GetAllAsync(GetAllPropertyItemRequestDto request)
    {
        var propertyItemSource = Context.PropertyItem
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter);

        var propertyItems =
            from propertyItem in propertyItemSource
            join property in Context.Property on propertyItem.PropertyId equals property.Id
            select new { property, propertyItem };

        propertyItems = propertyItems.ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Title))
            propertyItems = propertyItems.Where(x => x.propertyItem.Translations.Any(t => t.Title.Contains(request.Title)));

        if (!string.IsNullOrWhiteSpace(request.PropertyTitle))
            propertyItems = propertyItems.Where(x => x.property.Translations.Any(t => t.Title.Contains(request.PropertyTitle)));

        var result = await propertyItems
            .Select(x => new GetAllPropertyItemDto
            {
                Id = x.propertyItem.Id,
                Code = x.propertyItem.Code,
                Priority = x.propertyItem.Priority,
                IsActive = x.propertyItem.IsActive,
                PropertyId = x.propertyItem.PropertyId,
                PropertyCode = x.property.Code,
                PropertyType = x.property.PropertyType,
                PropertyCategoryId = x.property.PropertyCategoryId,
                Translations = x.propertyItem.Translations
                    .Select(t => new PropertyItemTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title
                    })
                    .ToList()
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
