using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionItems;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ISectionItemRepository
{
    Task<PagedResult<SectionItem>> GetAllAsync(GetAllSectionItemRequestDto request);
    Task<PagedResult<SectionItem>> SearchAsync(SearchSectionItemRequestDto request);
    void Add(SectionItem model);
    void Remove(SectionItem model);
    ValueTask<SectionItem?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<SectionItem?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<SectionItem, bool>> expression, CancellationToken cancellationToken = default);
}
