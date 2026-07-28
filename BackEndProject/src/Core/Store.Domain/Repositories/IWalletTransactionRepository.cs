using System.Linq.Expressions;
using Store.Domain.Dtos.Wallets;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IWalletTransactionRepository
{
    Task<PagedResult<WalletTransaction>> GetByUserAsync(
        int userId,
        GetWalletTransactionsRequestDto request);

    Task<PagedResult<WalletTransaction>> GetAllAsync(
        GetWalletTransactionsRequestDto request);
}
