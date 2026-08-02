using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.FinancialYears;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class FinancialYearRepository
    (EditionDbContext context)
    : Repository<FinancialYear>(context), IFinancialYearRepository
{    
    public async Task<PagedResult<FinancialYear>> GetAllAsync(GetAllFinancialYearRequestDto request)
    {
        var financialYears = Context.FinancialYear
            .AsQueryable()
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        var result =
            await financialYears
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<FinancialYear?> GetFirstActiveAsync(CancellationToken cancellationToken = default)
    {
        return await Context.FinancialYear
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}