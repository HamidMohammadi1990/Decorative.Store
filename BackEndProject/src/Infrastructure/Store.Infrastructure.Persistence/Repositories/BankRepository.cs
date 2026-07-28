using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Banks;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class BankRepository
    (EditionDbContext context)
    : Repository<Bank>(context), IBankRepository
{
    public async Task<PagedResult<Bank>> GetAllAsync(GetAllBankRequestDto request)
    {
        var banks = await Context.Bank
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return banks;
    }

    public async Task<PagedResult<Bank>> SearchAsync(SearchBankRequestDto request)
    {
        var banks = await Context.Bank
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
        
        return banks;
    }
}