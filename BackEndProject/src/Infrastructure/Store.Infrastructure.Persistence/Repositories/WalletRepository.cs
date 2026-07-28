using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Enums;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class WalletRepository
    (EditionDbContext context)
    : Repository<Wallet>(context), IWalletRepository
{
    public async Task<Wallet?> FindByUserIdAsync(int userId, int walletId, CancellationToken cancellationToken = default)
    {
        return await Context.Wallet
            .SingleOrDefaultAsync(x => x.Id == walletId && x.UserId == userId, cancellationToken);
    }

    public async Task<List<Wallet>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Wallet
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Wallet?> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Wallet
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.IsDefault)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> AnyDefaultWalletAsync(int userId, CancellationToken cancellationToken = default)
        => Context.Wallet.AnyAsync(x => x.UserId == userId && x.IsDefault, cancellationToken);

    public async Task<PagedResult<Wallet>> GetAllAsync(GetAllWalletRequestDto request)
    {
        return await Context.Wallet
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<Wallet>> SearchAsync(SearchWalletRequestDto request)
    {
        return await Context.Wallet
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.Status == WalletStatusType.Active)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToPagedAsync(request.Pagination);
    }

    public Task<bool> HasPendingBankChargeAsync(int walletId, CancellationToken cancellationToken = default)
    {
        return Context.WalletTransaction.AnyAsync(
            x => x.WalletId == walletId &&
                 x.Type == WalletTransactionType.Incremental &&
                 x.Status == WalletTransactionStatusType.Pending,
            cancellationToken);
    }

    public async Task ClearDefaultForUserAsync(int userId, int exceptWalletId, CancellationToken cancellationToken = default)
    {
        var wallets = await Context.Wallet
            .Where(x => x.UserId == userId && x.IsDefault && x.Id != exceptWalletId)
            .ToListAsync(cancellationToken);

        foreach (var wallet in wallets)
            wallet.SetDefault(false);
    }
}