using Store.Domain.Dtos.PageSections;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using System.Linq.Expressions;

namespace Store.Domain.Repositories;

public interface IPageSectionRepository
{
    Task<PagedResult<PageSection>> GetAllAsync(GetAllPageSectionRequestDto request);
    Task<PagedResult<PageSection>> SearchAsync(SearchPageSectionRequestDto request);
    void Add(PageSection model);
    void Remove(PageSection model);
    ValueTask<PageSection?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PageSection?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<PageSection, bool>> expression, CancellationToken cancellationToken = default);
}
