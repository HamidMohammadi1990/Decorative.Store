using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyCategories;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class PropertyCategoryRepository
    (EditionDbContext context)
    : Repository<PropertyCategory>(context), IPropertyCategoryRepository
{
    public async Task<PagedResult<PropertyCategory>> GetAllAsync(GetAllPropertyCategoryRequestDto request)
    {
        var categories = Context.PropertyCategory
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await categories
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}