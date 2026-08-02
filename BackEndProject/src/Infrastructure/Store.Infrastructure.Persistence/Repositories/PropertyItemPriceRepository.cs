using Edition.Application.Contracts.Localization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItemPrices;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyItemPriceRepository
    (EditionDbContext context, ICurrentLanguageContext languageContext, ILanguageRegistry languageRegistry)
    : Repository<PropertyItemPrice>(context), IPropertyItemPriceRepository
{
    public async Task<PagedResult<GetAllPropertyItemPriceDto>> GetAllAsync(GetAllPropertyItemPriceRequestDto request)
    {
        var defaultLanguage = await languageRegistry.GetDefaultAsync();
        var languageId = languageContext.IsResolved ? languageContext.LanguageId : defaultLanguage.Id;
        var defaultLanguageId = defaultLanguage.Id;

        var propertyItemPriceSource = Context.PropertyItemPrice
            .ApplyContentPolicyFilter(request.ContentFilter);

        var itemPrices =
            from propertyItemPrice in propertyItemPriceSource
            join propertyItem in Context.PropertyItem on propertyItemPrice.PropertyItemId equals propertyItem.Id
            join property in Context.Property on propertyItem.PropertyId equals property.Id
            join propertyCategory in Context.PropertyCategory on property.PropertyCategoryId equals propertyCategory.Id
            select new { propertyItemPrice, propertyItem, property, propertyCategory };

        itemPrices = itemPrices.ApplyQueryFilters(request);

        var result = await
            itemPrices
            .Select(x => new GetAllPropertyItemPriceDto
            {
                Id = x.propertyItemPrice.Id,
                Price = x.propertyItemPrice.Price,
                IsActive = x.propertyItemPrice.IsActive,
                PropertyId = x.property.Id,
                CreatedOnUtc = x.propertyItemPrice.CreatedOnUtc,
                PropertyTitle = x.property.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.property.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                PropertyItemId = x.propertyItemPrice.PropertyItemId,
                PropertyItemTitle = x.propertyItem.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.propertyItem.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CooperationPrice = x.propertyItemPrice.CooperationPrice,
                PropertyCategoryId = x.propertyCategory.Id,
                PropertyCategoryTitle = x.propertyCategory.Translations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? x.propertyCategory.Translations
                        .Where(t => t.LanguageId == defaultLanguageId)
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
