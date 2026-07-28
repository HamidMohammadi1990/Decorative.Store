using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPageRepository
{
    Task<PagedResult<Page>> GetAllAsync(GetAllPageRequestDto request);
    Task<PagedResult<Page>> SearchAsync(SearchPageRequestDto request);
    void Add(Page model);
    void Remove(Page model);
    ValueTask<Page?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> GetActiveBySlugWithContentAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Page, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> HasPageSectionsAsync(int pageId, CancellationToken cancellationToken = default);
}
