using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISectionTypeRepository
{
    Task<PagedResult<SectionType>> GetAllAsync(GetAllSectionTypeRequestDto request);
    Task<PagedResult<SectionType>> SearchAsync(SearchSectionTypeRequestDto request);
    void Add(SectionType model);
    void Remove(SectionType model);
    ValueTask<SectionType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<SectionType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<SectionType, bool>> expression, CancellationToken cancellationToken = default);
}
