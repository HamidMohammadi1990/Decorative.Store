using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPageRepository
{
    Task<PagedResult<GetAllPageResponseDto>> GetAllAsync(GetAllPageRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchPageResponseDto>> SearchAsync(SearchPageRequestDto request, CancellationToken cancellationToken = default);
    void Add(Page model);
    void Remove(Page model);
    ValueTask<Page?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Page?> GetActiveBySlugWithContentAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Page, bool>> expression, CancellationToken cancellationToken = default);
    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludePageId = null,
        CancellationToken cancellationToken = default);
    Task<bool> HasPageSectionsAsync(int pageId, CancellationToken cancellationToken = default);
}
