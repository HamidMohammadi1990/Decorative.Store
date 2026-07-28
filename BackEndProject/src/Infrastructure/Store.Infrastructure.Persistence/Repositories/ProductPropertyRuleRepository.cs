using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.ProductPropertyRules;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductPropertyRuleRepository
    (EditionDbContext context)
    : Repository<ProductPropertyRule>(context), IProductPropertyRuleRepository
{
    public async Task<PagedResult<ProductPropertyRule>> GetAllAsync(
        GetAllProductPropertyRuleRequestDto request)
    {
        var source = Context.ProductPropertyRule
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<ProductPropertyRule>> SearchAsync(
        SearchProductPropertyRuleRequestDto request)
    {
        var source = Context.ProductPropertyRule
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await source
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}