using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Tags;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ITagRepository
{
    Task<PagedResult<Tag>> GetAllAsync(GetAllTagRequestDto request);
    void Add(Tag tag);
    void Remove(Tag tag);
    ValueTask<Tag?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Tag?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Tag, bool>> expression, CancellationToken cancellationToken = default);
}