using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISectionRepository
{
    Task<PagedResult<GetAllSectionResponseDto>> GetAllAsync(GetAllSectionRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchSectionResponseDto>> SearchAsync(SearchSectionRequestDto request, CancellationToken cancellationToken = default);
    void Add(Section model);
    void Remove(Section model);
    ValueTask<Section?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Section?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Section?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<Section?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Section, bool>> expression, CancellationToken cancellationToken = default);
}
