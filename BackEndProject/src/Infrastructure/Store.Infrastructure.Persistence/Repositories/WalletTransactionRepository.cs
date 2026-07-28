using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class WalletTransactionRepository
    (EditionDbContext context)
    : Repository<WalletTransaction>(context), IWalletTransactionRepository
{
    public async Task<PagedResult<WalletTransaction>> GetByUserAsync(
        int userId,
        GetWalletTransactionsRequestDto request)
    {
        return await Context.WalletTransaction
            .Include(x => x.Wallet)
            .Where(x => x.Wallet.UserId == userId)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<WalletTransaction>> GetAllAsync(
        GetWalletTransactionsRequestDto request)
    {
        return await Context.WalletTransaction
            .Include(x => x.Wallet)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToPagedAsync(request.Pagination);
    }
}
