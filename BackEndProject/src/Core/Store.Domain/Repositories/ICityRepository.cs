using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Cities;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICityRepository
{
    ValueTask<City?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<City?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Add(City city);
    Task<bool> AnyAsync(Expression<Func<City, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllCityResponseDto>> GetAllAsync(GetAllCityRequestDto request);
    Task<PagedResult<SearchCityResponseDto>> SearchAsync(SearchCityRequestDto request);
}