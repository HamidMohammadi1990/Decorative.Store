using System.Linq.Expressions;
using Store.Domain.Dtos.Banks;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBankRepository
{
    Task<PagedResult<Bank>> GetAllAsync(GetAllBankRequestDto request);
    Task<PagedResult<Bank>> SearchAsync(SearchBankRequestDto request);
    void Add(Bank bank);
    void Remove(Bank bank);
    ValueTask<Bank?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Bank?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Bank, bool>> expression, CancellationToken cancellationToken = default);
}