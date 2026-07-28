using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.FinancialYears;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IFinancialYearRepository
{
    void Add(FinancialYear chartOfAccount);
    Task<PagedResult<FinancialYear>> GetAllAsync(GetAllFinancialYearRequestDto request);
    ValueTask<FinancialYear?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<FinancialYear?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<FinancialYear?> GetByCompanyIdAsync(int companyId);

    Task<FinancialYear?> GetFirstActiveAsync(CancellationToken cancellationToken = default);
}