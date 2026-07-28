using Store.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Properties;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyRepository
    (EditionDbContext context)
    : Repository<Property>(context), IPropertyRepository
{
    public async Task<PagedResult<GetAllPropertyDto>> GetAllAsync(GetAllPropertyRequestDto request)
    {
        var propertySource = Context.Property
            .ApplyContentPolicyFilter(request.ContentFilter);

        var properties =
            from property in propertySource
            join propertyCategory in Context.PropertyCategory on property.PropertyCategoryId equals propertyCategory.Id
            select new { property, propertyCategory };

        properties = properties.ApplyQueryFilters(request);

        var result = await
            properties
            .Select(x => new GetAllPropertyDto
            {
                Id = x.property.Id,
                Title = x.property.Title,
                Priority = x.property.Priority,
                ParentId = x.property.ParentId,
                IsActive = x.property.IsActive,
                Description = x.property.Description,
                PropertyType = x.property.PropertyType,
                PropertyCategoryId = x.property.PropertyCategoryId,
                PropertyCategoryTitle = x.propertyCategory.Title
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<ProductPropertyDto>> GetByProductIdAsync(int productId, int companyId)
    {
        var properties =
            await (from ProductProperty in Context.ProductProperty
                   join Property in Context.Property on
                        new { ProductProperty.PropertyId, ProductProperty.ProductId, ProductProperty.IsActive } equals
                        new { PropertyId = Property.Id, ProductId = productId, Property.IsActive }

                   join PropertyPrice in Context.ProductPropertyPrice on
                        new { ProductPropertyId = ProductProperty.Id, ProductProperty.IsActive, CompanyId = companyId } equals
                        new { PropertyPrice.ProductPropertyId, PropertyPrice.IsActive, PropertyPrice.CompanyId }
                   into PropertyPrices
                   from PropertyPrice in PropertyPrices.DefaultIfEmpty()

                   join PropertyCategory in Context.PropertyCategory on
                        new { Property.PropertyCategoryId, Property.IsActive } equals
                        new { PropertyCategoryId = PropertyCategory.Id, PropertyCategory.IsActive }

                   join ParentProperty in Context.Property on
                        new { ParentPropertyId = Property.Id, Property.IsActive } equals
                        new { ParentPropertyId = ParentProperty.ParentId ?? 0, ParentProperty.IsActive }
                   into ParentProperties
                   from ParentProperty in ParentProperties.DefaultIfEmpty()

                   join ParentProductProperty in Context.ProductProperty on
                        new { PropertyId = ParentProperty.Id, ProductId = productId, ParentProperty.IsActive } equals
                        new { ParentProductProperty.PropertyId, ParentProductProperty.ProductId, ParentProductProperty.IsActive }
                   into ParentProductProperties
                   from ParentProductProperty in ParentProductProperties.DefaultIfEmpty()

                   join ParentPropertyPrice in Context.ProductPropertyPrice on
                        new { ProductPropertyId = ParentProductProperty.Id, ParentProductProperty.IsActive, CompanyId = companyId } equals
                        new { ParentPropertyPrice.ProductPropertyId, ParentPropertyPrice.IsActive, ParentPropertyPrice.CompanyId }
                   into ParentPropertyPrices
                   from ParentPropertyPrice in ParentPropertyPrices.DefaultIfEmpty()

                   join PropertyItem in Context.PropertyItem on
                        new { PropertyId = Property.Id, Property.IsActive } equals
                        new { PropertyItem.PropertyId, PropertyItem.IsActive }
                   into PropertyItems
                   from PropertyItem in PropertyItems.DefaultIfEmpty()

                   join PropertyItemPrice in Context.PropertyItemPrice on
                        new { PropertyItemId = PropertyItem.Id, CompanyId = companyId, PropertyItem.IsActive } equals
                        new { PropertyItemPrice.PropertyItemId, PropertyItemPrice.CompanyId, PropertyItemPrice.IsActive }
                   into PropertyItemPrices
                   from PropertyItemPrice in PropertyItemPrices.DefaultIfEmpty()

                   join ParentPropertyItem in Context.PropertyItem on
                        new { PropertyId = ParentProperty.Id, ParentProperty.IsActive } equals
                        new { ParentPropertyItem.PropertyId, ParentPropertyItem.IsActive }
                   into ParentPropertyItems
                   from ParentPropertyItem in ParentPropertyItems.DefaultIfEmpty()

                   join ParentPropertyItemPrice in Context.PropertyItemPrice on
                        new { PropertyItemId = ParentPropertyItem.Id, CompanyId = companyId, ParentPropertyItem.IsActive } equals
                        new { ParentPropertyItemPrice.PropertyItemId, ParentPropertyItemPrice.CompanyId, ParentPropertyItemPrice.IsActive }
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

                   select new ProductPropertyDto
                   {
                       // ProductProperty
                       ProductPropertyId = ProductProperty.Id,

                       // Parent ProductProperty
                       ParentProductPropertyId = ParentProductProperty.Id,

                       // Property
                       CategoryTitle = PropertyCategory.Title,
                       PropertyId = Property.Id,
                       PropertyTitle = Property.Title,
                       PropertyPriority = Property.Priority,
                       PropertyType = Property.PropertyType,
                       PropertyParentId = Property.ParentId,
                       PropertyPriceId = PropertyPrice.Id,
                       PropertyPrice = PropertyPrice.Price,
                       PropertyCooperationPrice = PropertyPrice.CooperationPrice,

                       // Parent Property
                       ParentPropertyId = ParentProperty.Id,
                       ParentPropertyTitle = ParentProperty.Title,
                       ParentPropertyPriority = Property.Priority,
                       ParentPropertyType = Property.PropertyType,
                       ParentPropertyPriceId = ParentPropertyPrice.Id,
                       ParentPropertyPrice = ParentPropertyPrice.Price,
                       ParentPropertyCooperationPrice = ParentPropertyPrice.CooperationPrice,

                       // Property item
                       PropertyItemId = PropertyItem.Id,
                       PropertyItemPropertyId = PropertyItem.PropertyId,
                       PropertyItemTitle = PropertyItem.Title,
                       PropertyItemPriority = PropertyItem.Priority,
                       PropertyItemPriceId = PropertyItemPrice.Id,
                       PropertyItemPrice = PropertyItemPrice.Price,
                       PropertyItemCooperationPrice = PropertyItemPrice.CooperationPrice,

                       // Dependency Property item
                       DependencyParentPropertyItemId = PropertyItemDependency.ParentPropertyItemId,
                       DependencyDependentPropertyItemId = PropertyItemDependency.DependentPropertyItemId,

                       // Parent Property item
                       ParentPropertyItemId = ParentPropertyItem.Id,
                       ParentPropertyItemPropertyId = ParentPropertyItem.PropertyId,
                       ParentPropertyItemTitle = ParentPropertyItem.Title,
                       ParentPropertyItemPriority = ParentPropertyItem.Priority,
                       ParentPropertyItemPriceId = ParentPropertyItemPrice.Id,
                       ParentPropertyItemPrice = ParentPropertyItemPrice.Price,
                       ParentPropertyItemCooperationPrice = ParentPropertyItemPrice.CooperationPrice,

                       // Property Rule
                       PropertyRuleProductPropertyId = ProductPropertyRule.ProductPropertyId,
                       PropertyRuleIsMandatory = ProductPropertyRule.IsMandatory,
                       PropertyRuleDescription = ProductPropertyRule.Description,
                       PropertyRulePropertyType = ProductPropertyRule.PropertyType,
                       PropertyRuleMinQuantity = ((NumericProductPropertyRule)ProductPropertyRule).MinQuantity,
                       PropertyRuleMaxQuantity = ((NumericProductPropertyRule)ProductPropertyRule).MaxQuantity,
                       PropertyRuleMinWidth = ((DimensionsProductPropertyRule)ProductPropertyRule).MinWidth,
                       PropertyRuleMaxWidth = ((DimensionsProductPropertyRule)ProductPropertyRule).MaxWidth,
                       PropertyRuleMinHeight = ((DimensionsProductPropertyRule)ProductPropertyRule).MinHeight,
                       PropertyRuleMaxHeight = ((DimensionsProductPropertyRule)ProductPropertyRule).MaxHeight,
                       PropertyRuleMinLength = ((TextProductPropertyRule)ProductPropertyRule).MinLength,
                       PropertyRuleMaxLength = ((TextProductPropertyRule)ProductPropertyRule).MaxLength,

                       // Parent Property rule
                       ParentPropertyRuleProductPropertyId = ParentProductPropertyRule.ProductPropertyId,
                       ParentPropertyRuleIsMandatory = ParentProductPropertyRule.IsMandatory,
                       ParentPropertyRuleDescription = ParentProductPropertyRule.Description,
                       ParentPropertyRulePropertyType = ParentProductPropertyRule.PropertyType,
                       ParentPropertyRuleMinQuantity = ((NumericProductPropertyRule)ParentProductPropertyRule).MinQuantity,
                       ParentPropertyRuleMaxQuantity = ((NumericProductPropertyRule)ParentProductPropertyRule).MaxQuantity,
                       ParentPropertyRuleMinWidth = ((DimensionsProductPropertyRule)ParentProductPropertyRule).MinWidth,
                       ParentPropertyRuleMaxWidth = ((DimensionsProductPropertyRule)ParentProductPropertyRule).MaxWidth,
                       ParentPropertyRuleMinHeight = ((DimensionsProductPropertyRule)ParentProductPropertyRule).MinHeight,
                       ParentPropertyRuleMaxHeight = ((DimensionsProductPropertyRule)ParentProductPropertyRule).MaxHeight,
                       ParentPropertyRuleMinLength = ((TextProductPropertyRule)ParentProductPropertyRule).MinLength,
                       ParentPropertyRuleMaxLength = ((TextProductPropertyRule)ParentProductPropertyRule).MaxLength,
                   })
                    .AsNoTracking()
                    .ToListAsync();

        return properties;
    }
}