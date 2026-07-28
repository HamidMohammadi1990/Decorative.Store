using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISectionRepository
{
    Task<PagedResult<Section>> GetAllAsync(GetAllSectionRequestDto request);
    Task<PagedResult<Section>> SearchAsync(SearchSectionRequestDto request);
    void Add(Section model);
    void Remove(Section model);
    ValueTask<Section?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Section?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Section, bool>> expression, CancellationToken cancellationToken = default);
}
