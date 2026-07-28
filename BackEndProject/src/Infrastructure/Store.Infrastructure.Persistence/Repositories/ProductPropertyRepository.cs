using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductProperties;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductPropertyRepository
    (EditionDbContext context)
    : Repository<ProductProperty>(context), IProductPropertyRepository
{
    public async Task<PagedResult<ProductProperty>> GetAllAsync(
        GetAllProductPropertyRequestDto request)
    {
        var source = Context.ProductProperty
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<ProductProperty>> SearchAsync(
        SearchProductPropertyRequestDto request)
    {
        var source = Context.ProductProperty
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}
