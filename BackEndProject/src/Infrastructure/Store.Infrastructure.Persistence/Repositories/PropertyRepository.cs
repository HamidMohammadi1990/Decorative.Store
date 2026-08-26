using Edition.Application.Contracts.Localization;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Enums;
using System.Linq.Expressions;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Properties;
using Store.Domain.Dtos.Localization;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<Property>(context), IPropertyRepository
{
    public Task<Property?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => Context.Property
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Property?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default)
        => Context.Property
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsCodeAsync(
        string code,
        int propertyCategoryId,
        int? excludePropertyId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();
        return Context.Property.AnyAsync(
            x => x.Code == normalizedCode &&
                 x.PropertyCategoryId == propertyCategoryId &&
                 (!excludePropertyId.HasValue || x.Id != excludePropertyId.Value),
            cancellationToken);
    }

    public async Task<PagedResult<GetAllPropertyDto>> GetAllAsync(
        GetAllPropertyRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var propertySource = Context.Property
            .AsNoTracking()
            .Include(x => x.Translations)
            .ApplyContentPolicyFilter(request.ContentFilter);

        var properties =
            from property in propertySource
            join propertyCategory in Context.PropertyCategory on property.PropertyCategoryId equals propertyCategory.Id
            select new { property, propertyCategory };

        properties = properties.ApplyQueryFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Title))
            properties = properties.Where(x => x.property.Translations.Any(t => t.Title.Contains(request.Title)));

        var result = await properties
            .Select(x => new GetAllPropertyDto
            {
                Id = x.property.Id,
                Code = x.property.Code,
                Priority = x.property.Priority,
                ParentId = x.property.ParentId,
                IsActive = x.property.IsActive,
                PropertyType = x.property.PropertyType,
                PropertyCategoryId = x.property.PropertyCategoryId,
                PropertyCategoryCode = x.propertyCategory.Code,
                Translations = x.property.Translations
                    .Select(t => new PropertyTranslationItemDto
                    {
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Description = t.Description
                    })
                    .ToList()
            })
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<ProductPropertyDto>> GetByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync(cancellationToken);
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var defaultLanguageId = defaultLanguage.Id;

        var properties =
            await (from ProductProperty in Context.ProductProperty
                   join Property in Context.Property on
                        new { ProductProperty.PropertyId, ProductProperty.ProductId, ProductProperty.IsActive } equals
                        new { PropertyId = Property.Id, ProductId = productId, Property.IsActive }

                   join PropertyCategory in Context.PropertyCategory on
                        new { Property.PropertyCategoryId, Property.IsActive } equals
                        new { PropertyCategoryId = PropertyCategory.Id, PropertyCategory.IsActive }

                   join ParentProperty in Context.Property on
                        new { Id = Property.ParentId ?? 0, Property.IsActive } equals
                        new { ParentProperty.Id, ParentProperty.IsActive }
                   into ParentProperties
                   from ParentProperty in ParentProperties.DefaultIfEmpty()

                   join ParentProductProperty in Context.ProductProperty on
                        new { PropertyId = ParentProperty.Id, ProductId = productId, ParentProperty.IsActive } equals
                        new { ParentProductProperty.PropertyId, ParentProductProperty.ProductId, ParentProductProperty.IsActive }
                   into ParentProductProperties
                   from ParentProductProperty in ParentProductProperties.DefaultIfEmpty()

                   join PropertyItem in Context.PropertyItem on
                        new { PropertyId = Property.Id, Property.IsActive } equals
                        new { PropertyItem.PropertyId, PropertyItem.IsActive }
                   into PropertyItems
                   from PropertyItem in PropertyItems.DefaultIfEmpty()

                   join ParentPropertyItem in Context.PropertyItem on
                        new { PropertyId = ParentProperty.Id, ParentProperty.IsActive } equals
                        new { ParentPropertyItem.PropertyId, ParentPropertyItem.IsActive }
                   into ParentPropertyItems
                   from ParentPropertyItem in ParentPropertyItems.DefaultIfEmpty()

                   join PropertyItemDependency in Context.PropertyItemDependency on PropertyItem.Id equals PropertyItemDependency.ParentPropertyItemId
                   into PropertyItemDependencies
                   from PropertyItemDependency in PropertyItemDependencies.DefaultIfEmpty()

                   select new ProductPropertyDto
                   {
                       ProductPropertyId = ProductProperty.Id,
                       ParentProductPropertyId = (int?)ParentProductProperty.Id,
                       CategoryTitle = PropertyCategory.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Title)
                           .FirstOrDefault()
                           ?? PropertyCategory.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Title)
                               .FirstOrDefault()
                           ?? string.Empty,
                       PropertyId = Property.Id,
                       PropertyTitle = Property.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Title)
                           .FirstOrDefault()
                           ?? Property.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Title)
                               .FirstOrDefault()
                           ?? string.Empty,
                       PropertyPriority = Property.Priority,
                       PropertyType = Property.PropertyType,
                       PropertyParentId = Property.ParentId,
                       ParentPropertyId = (int?)ParentProperty.Id,
                       ParentPropertyTitle = ParentProperty.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Title)
                           .FirstOrDefault()
                           ?? ParentProperty.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Title)
                               .FirstOrDefault()
                           ?? string.Empty,
                       ParentPropertyPriority = (int?)ParentProperty.Priority,
                       ParentPropertyType = (PropertyType?)ParentProperty.PropertyType,
                       PropertyItemId = (int?)PropertyItem.Id,
                       PropertyItemPropertyId = (int?)PropertyItem.PropertyId,
                       PropertyItemTitle = PropertyItem.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Title)
                           .FirstOrDefault()
                           ?? PropertyItem.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Title)
                               .FirstOrDefault()
                           ?? string.Empty,
                       PropertyItemPriority = (int?)PropertyItem.Priority,
                       DependencyParentPropertyItemId = (int?)PropertyItemDependency.ParentPropertyItemId,
                       DependencyDependentPropertyItemId = (int?)PropertyItemDependency.DependentPropertyItemId,
                       ParentPropertyItemId = (int?)ParentPropertyItem.Id,
                       ParentPropertyItemPropertyId = (int?)ParentPropertyItem.PropertyId,
                       ParentPropertyItemTitle = ParentPropertyItem.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Title)
                           .FirstOrDefault()
                           ?? ParentPropertyItem.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Title)
                               .FirstOrDefault()
                           ?? string.Empty,
                       ParentPropertyItemPriority = (int?)ParentPropertyItem.Priority,
                   })
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        return properties;
    }
}
