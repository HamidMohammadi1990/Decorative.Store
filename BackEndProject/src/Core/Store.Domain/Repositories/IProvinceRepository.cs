using System.Linq.Expressions;
using Store.Domain.Dtos.Provinces;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProvinceRepository
{
    void Add(Province province);
    ValueTask<Province?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Province?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Province, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProvinceResponseDto>> GetAllAsync(GetAllProvinceRequestDto request);
    Task<PagedResult<SearchProvinceResponseDto>> SearchAsync(SearchProvinceRequestDto request);
}