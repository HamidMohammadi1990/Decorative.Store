using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ChartOfAccounts;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IChartOfAccountRepository
{
    Task<ChartOfAccount?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Add(ChartOfAccount chartOfAccount);
    void Remove(ChartOfAccount chartOfAccount);
    ValueTask<ChartOfAccount?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllChartOfAccountDto>> GetAllAsync(GetAllChartOfAccountRequestDto request);
}