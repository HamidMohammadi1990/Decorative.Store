using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFeatureTypes;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductFeatureTypeRepository
    (EditionDbContext context)
    : Repository<ProductFeatureType>(context), IProductFeatureTypeRepository
{
    public async Task<PagedResult<ProductFeatureType>> GetAllAsync(
        GetAllProductFeatureTypeRequestDto request)
    {
        var source = Context.ProductFeatureType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<ProductFeatureType>> SearchAsync(
        SearchProductFeatureTypeRequestDto request)
    {
        var source = Context.ProductFeatureType
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}