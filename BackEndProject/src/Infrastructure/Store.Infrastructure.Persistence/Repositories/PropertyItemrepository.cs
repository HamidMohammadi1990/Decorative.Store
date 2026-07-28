using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItems;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyItemRepository
    (EditionDbContext context)
    : Repository<PropertyItem>(context), IPropertyItemRepository
{
    public async Task<PagedResult<GetAllPropertyItemDto>> GetAllAsync(GetAllPropertyItemRequestDto request)
    {
        var propertyItemSource = Context.PropertyItem
            .ApplyContentPolicyFilter(request.ContentFilter);

        var propertyItems =
            from propertyItem in propertyItemSource
            join property in Context.Property on propertyItem.PropertyId equals property.Id
            select new { property, propertyItem };

        propertyItems = propertyItems.ApplyQueryFilters(request);

        var result = await propertyItems
            .Select(x => new GetAllPropertyItemDto
            {
                Id = x.propertyItem.Id,
                Title = x.propertyItem.Title,
                Priority = x.propertyItem.Priority,
                IsActive = x.propertyItem.IsActive,
                PropertyId = x.propertyItem.PropertyId,
                PropertyType = x.property.PropertyType,
                PropertyTitle = x.property.Title,
                PropertyCategoryId = x.property.PropertyCategoryId
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}