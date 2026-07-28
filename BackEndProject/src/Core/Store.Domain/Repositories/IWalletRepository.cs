using System.Linq.Expressions;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IWalletRepository
{
    void Add(Wallet wallet);

    Task<Wallet?> FindByUserIdAsync(int userId, int walletId, CancellationToken cancellationToken = default);

    Task<List<Wallet>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<Wallet?> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<bool> AnyDefaultWalletAsync(int userId, CancellationToken cancellationToken = default);

    Task<PagedResult<Wallet>> GetAllAsync(GetAllWalletRequestDto request);

    Task<PagedResult<Wallet>> SearchAsync(SearchWalletRequestDto request);

    Task<Wallet?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<Wallet?> FindAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> HasPendingBankChargeAsync(int walletId, CancellationToken cancellationToken = default);

    Task ClearDefaultForUserAsync(int userId, int exceptWalletId, CancellationToken cancellationToken = default);
}
