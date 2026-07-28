using System.Linq.Expressions;
using Store.Domain.Dtos.Companies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICompanyRepository
{
    void Add(Company company);
    ValueTask<Company?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Company?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Company, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCompanyResponseDto>> GetAllAsync(GetAllCompanyRequestDto request);
    Task<PagedResult<SearchCompanyResponseDto>> SearchAsync(SearchCompanyRequestDto request);
}