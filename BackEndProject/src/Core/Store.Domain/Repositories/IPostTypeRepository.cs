using System.Linq.Expressions;
using Store.Domain.Dtos.PostTypes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IPostTypeRepository
{
    Task<List<PostType>> GetAllAsync();
    void Add(PostType postType);
    void Remove(PostType postType);
    ValueTask<PostType?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PostType?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<PostType, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllPostTypeResponseDto>> GetAllAsync(GetAllPostTypeRequestDto request);
    Task<PagedResult<SearchPostTypeResponseDto>> SearchAsync(SearchPostTypeRequestDto request);
}