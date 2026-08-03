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

                   join PropertyPrice in Context.ProductPropertyPrice on
                        new { ProductPropertyId = ProductProperty.Id, ProductProperty.IsActive } equals
                        new { PropertyPrice.ProductPropertyId, PropertyPrice.IsActive }
                   into PropertyPrices
                   from PropertyPrice in PropertyPrices.DefaultIfEmpty()

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

                   join ParentPropertyPrice in Context.ProductPropertyPrice on
                        new { ProductPropertyId = ParentProductProperty.Id, ParentProductProperty.IsActive } equals
                        new { ParentPropertyPrice.ProductPropertyId, ParentPropertyPrice.IsActive }
                   into ParentPropertyPrices
                   from ParentPropertyPrice in ParentPropertyPrices.DefaultIfEmpty()

                   join PropertyItem in Context.PropertyItem on
                        new { PropertyId = Property.Id, Property.IsActive } equals
                        new { PropertyItem.PropertyId, PropertyItem.IsActive }
                   into PropertyItems
                   from PropertyItem in PropertyItems.DefaultIfEmpty()

                   join PropertyItemPrice in Context.PropertyItemPrice on
                        new { PropertyItemId = PropertyItem.Id, PropertyItem.IsActive } equals
                        new { PropertyItemPrice.PropertyItemId, PropertyItemPrice.IsActive }
                   into PropertyItemPrices
                   from PropertyItemPrice in PropertyItemPrices.DefaultIfEmpty()

                   join ParentPropertyItem in Context.PropertyItem on
                        new { PropertyId = ParentProperty.Id, ParentProperty.IsActive } equals
                        new { ParentPropertyItem.PropertyId, ParentPropertyItem.IsActive }
                   into ParentPropertyItems
                   from ParentPropertyItem in ParentPropertyItems.DefaultIfEmpty()

                   join ParentPropertyItemPrice in Context.PropertyItemPrice on
                        new { PropertyItemId = ParentPropertyItem.Id, ParentPropertyItem.IsActive } equals
                        new { ParentPropertyItemPrice.PropertyItemId, ParentPropertyItemPrice.IsActive }
                   into ParentPropertyItemPrices
                   from ParentPropertyItemPrice in ParentPropertyItemPrices.DefaultIfEmpty()

                   join ProductPropertyRule in Context.ProductPropertyRule on
                        new { ProductPropertyId = ProductProperty.Id, ProductProperty.IsActive } equals
                        new { ProductPropertyRule.ProductPropertyId, ProductPropertyRule.IsActive }
                   into ProductPropertyRules
                   from ProductPropertyRule in ProductPropertyRules.DefaultIfEmpty()

                   join ParentProductPropertyRule in Context.ProductPropertyRule on
                        new { ProductPropertyId = ParentProductProperty.Id, ParentProductProperty.IsActive } equals
                        new { ParentProductPropertyRule.ProductPropertyId, ParentProductPropertyRule.IsActive }
                   into ParentProductPropertyRules
                   from ParentProductPropertyRule in ParentProductPropertyRules.DefaultIfEmpty()

                   join PropertyItemDependency in Context.PropertyItemDependency on PropertyItem.Id equals PropertyItemDependency.ParentPropertyItemId
                   into PropertyItemDependencies
                   from PropertyItemDependency in PropertyItemDependencies.DefaultIfEmpty()

                   select new
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
                       PropertyPriceId = (int?)PropertyPrice.Id,
                       PropertyPrice = (decimal?)PropertyPrice.Price,
                       PropertyCooperationPrice = (decimal?)PropertyPrice.CooperationPrice,
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
                       ParentPropertyPriceId = (int?)ParentPropertyPrice.Id,
                       ParentPropertyPrice = (decimal?)ParentPropertyPrice.Price,
                       ParentPropertyCooperationPrice = (decimal?)ParentPropertyPrice.CooperationPrice,
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
                       PropertyItemPriceId = (int?)PropertyItemPrice.Id,
                       PropertyItemPrice = (decimal?)PropertyItemPrice.Price,
                       PropertyItemCooperationPrice = (decimal?)PropertyItemPrice.CooperationPrice,
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
                       ParentPropertyItemPriceId = (int?)ParentPropertyItemPrice.Id,
                       ParentPropertyItemPrice = (decimal?)ParentPropertyItemPrice.Price,
                       ParentPropertyItemCooperationPrice = (decimal?)ParentPropertyItemPrice.CooperationPrice,
                       PropertyRuleProductPropertyId = (int?)ProductPropertyRule.ProductPropertyId,
                       PropertyRuleIsMandatory = (bool?)ProductPropertyRule.IsMandatory,
                       PropertyRuleDescription = ProductPropertyRule.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Description)
                           .FirstOrDefault()
                           ?? ProductPropertyRule.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Description)
                               .FirstOrDefault(),
                       PropertyRulePropertyType = (PropertyType?)ProductPropertyRule.PropertyType,
                       ParentPropertyRuleProductPropertyId = (int?)ParentProductPropertyRule.ProductPropertyId,
                       ParentPropertyRuleIsMandatory = (bool?)ParentProductPropertyRule.IsMandatory,
                       ParentPropertyRuleDescription = ParentProductPropertyRule.Translations
                           .Where(t => t.LanguageId == languageId)
                           .Select(t => t.Description)
                           .FirstOrDefault()
                           ?? ParentProductPropertyRule.Translations
                               .Where(t => t.LanguageId == defaultLanguageId)
                               .Select(t => t.Description)
                               .FirstOrDefault(),
                       ParentPropertyRulePropertyType = (PropertyType?)ParentProductPropertyRule.PropertyType,
                   })
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        var productPropertyIds = properties
            .SelectMany(x => new[] { x.PropertyRuleProductPropertyId, x.ParentPropertyRuleProductPropertyId })
            .Where(id => id.HasValue && id.Value > 0)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        var rulePayloads = productPropertyIds.Count == 0
            ? new Dictionary<int, ProductPropertyRulePayload>()
            : await Context.ProductPropertyRule
                .AsNoTracking()
                .Where(rule => productPropertyIds.Contains(rule.ProductPropertyId) && rule.IsActive)
                .ToDictionaryAsync(rule => rule.ProductPropertyId, rule => rule.Get(), cancellationToken);

        return properties
            .Select(x => new ProductPropertyDto
            {
                ProductPropertyId = x.ProductPropertyId,
                ParentProductPropertyId = x.ParentProductPropertyId,
                CategoryTitle = x.CategoryTitle,
                PropertyId = x.PropertyId,
                PropertyTitle = x.PropertyTitle,
                PropertyPriority = x.PropertyPriority,
                PropertyType = x.PropertyType,
                PropertyParentId = x.PropertyParentId,
                PropertyPriceId = x.PropertyPriceId,
                PropertyPrice = x.PropertyPrice,
                PropertyCooperationPrice = x.PropertyCooperationPrice,
                ParentPropertyId = x.ParentPropertyId,
                ParentPropertyTitle = x.ParentPropertyTitle,
                ParentPropertyPriority = x.ParentPropertyPriority,
                ParentPropertyType = x.ParentPropertyType,
                ParentPropertyPriceId = x.ParentPropertyPriceId,
                ParentPropertyPrice = x.ParentPropertyPrice,
                ParentPropertyCooperationPrice = x.ParentPropertyCooperationPrice,
                PropertyItemId = x.PropertyItemId,
                PropertyItemPropertyId = x.PropertyItemPropertyId,
                PropertyItemTitle = x.PropertyItemTitle,
                PropertyItemPriority = x.PropertyItemPriority,
                PropertyItemPriceId = x.PropertyItemPriceId,
                PropertyItemPrice = x.PropertyItemPrice,
                PropertyItemCooperationPrice = x.PropertyItemCooperationPrice,
                DependencyParentPropertyItemId = x.DependencyParentPropertyItemId,
                DependencyDependentPropertyItemId = x.DependencyDependentPropertyItemId,
                ParentPropertyItemId = x.ParentPropertyItemId,
                ParentPropertyItemPropertyId = x.ParentPropertyItemPropertyId,
                ParentPropertyItemTitle = x.ParentPropertyItemTitle,
                ParentPropertyItemPriority = x.ParentPropertyItemPriority,
                ParentPropertyItemPriceId = x.ParentPropertyItemPriceId,
                ParentPropertyItemPrice = x.ParentPropertyItemPrice,
                ParentPropertyItemCooperationPrice = x.ParentPropertyItemCooperationPrice,
                PropertyRuleProductPropertyId = x.PropertyRuleProductPropertyId,
                PropertyRuleIsMandatory = x.PropertyRuleIsMandatory,
                PropertyRuleDescription = x.PropertyRuleDescription,
                PropertyRulePropertyType = x.PropertyRulePropertyType,
                PropertyRuleMinQuantity = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MinQuantity,
                PropertyRuleMaxQuantity = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MaxQuantity,
                PropertyRuleMinWidth = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MinWidth,
                PropertyRuleMaxWidth = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MaxWidth,
                PropertyRuleMinHeight = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MinHeight,
                PropertyRuleMaxHeight = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MaxHeight,
                PropertyRuleMinLength = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MinLength,
                PropertyRuleMaxLength = GetRulePayload(x.PropertyRuleProductPropertyId, rulePayloads)?.MaxLength,
                ParentPropertyRuleProductPropertyId = x.ParentPropertyRuleProductPropertyId,
                ParentPropertyRuleIsMandatory = x.ParentPropertyRuleIsMandatory,
                ParentPropertyRuleDescription = x.ParentPropertyRuleDescription,
                ParentPropertyRulePropertyType = x.ParentPropertyRulePropertyType,
                ParentPropertyRuleMinQuantity = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MinQuantity,
                ParentPropertyRuleMaxQuantity = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MaxQuantity,
                ParentPropertyRuleMinWidth = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MinWidth,
                ParentPropertyRuleMaxWidth = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MaxWidth,
                ParentPropertyRuleMinHeight = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MinHeight,
                ParentPropertyRuleMaxHeight = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MaxHeight,
                ParentPropertyRuleMinLength = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MinLength,
                ParentPropertyRuleMaxLength = GetRulePayload(x.ParentPropertyRuleProductPropertyId, rulePayloads)?.MaxLength,
            })
            .ToList();
    }

    private static ProductPropertyRulePayload? GetRulePayload(
        int? productPropertyId,
        IReadOnlyDictionary<int, ProductPropertyRulePayload> rulePayloads)
        => productPropertyId is > 0 && rulePayloads.TryGetValue(productPropertyId.Value, out var payload)
            ? payload
            : null;
}
