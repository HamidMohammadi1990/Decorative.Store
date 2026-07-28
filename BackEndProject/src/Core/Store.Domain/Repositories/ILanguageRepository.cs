using System.Linq.Expressions;
using Store.Domain.Dtos.Languages;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ILanguageRepository
{
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Language, bool>> expression, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Language>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<Language>> GetAllAsync(GetAllLanguageRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<Language>> SearchAsync(SearchLanguageRequestDto request, CancellationToken cancellationToken = default);
    Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Language?> GetDefaultAsync(CancellationToken cancellationToken = default);
    ValueTask<Language?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Language?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Add(Language language);
    void AddRange(IEnumerable<Language> languages);
    void Remove(Language language);
    Task ClearDefaultAsync(int exceptLanguageId, CancellationToken cancellationToken = default);
}
