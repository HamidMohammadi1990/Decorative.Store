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
    (EditionDbContext context)
    : Repository<PropertyItemPrice>(context), IPropertyItemPriceRepository
{
    public async Task<PagedResult<GetAllPropertyItemPriceDto>> GetAllAsync(GetAllPropertyItemPriceRequestDto request)
    {
        var propertyItemPriceSource = Context.PropertyItemPrice
            .ApplyContentPolicyFilter(request.ContentFilter);

        var itemPrices =
            from propertyItemPrice in propertyItemPriceSource
            join company in Context.Company on propertyItemPrice.CompanyId equals company.Id
            join propertyItem in Context.PropertyItem on propertyItemPrice.PropertyItemId equals propertyItem.Id
            join property in Context.Property on propertyItem.PropertyId equals property.Id
            join propertyCategory in Context.PropertyCategory on property.PropertyCategoryId equals propertyCategory.Id
            select new { propertyItemPrice, company, propertyItem, property, propertyCategory };

        itemPrices = itemPrices.ApplyQueryFilters(request);

        var result = await
            itemPrices
            .Select(x => new GetAllPropertyItemPriceDto
            {
                Id = x.propertyItemPrice.Id,
                Price = x.propertyItemPrice.Price,
                IsActive = x.propertyItemPrice.IsActive,
                CompanyId = x.propertyItemPrice.CompanyId,
                PropertyId = x.property.Id,
                CompanyName = x.company.Name,
                CreatedOnUtc = x.propertyItemPrice.CreatedOnUtc,
                PropertyTitle = x.property.Title,
                PropertyItemId = x.propertyItemPrice.PropertyItemId,
                PropertyItemTitle = x.propertyItem.Title,
                CooperationPrice = x.propertyItemPrice.CooperationPrice,
                PropertyCategoryId = x.propertyCategory.Id,
                PropertyCategoryTitle = x.propertyCategory.Title,
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}