using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostTags;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostTagRepository
{
    Task<PagedResult<GetAllBlogPostTagDto>> GetAllAsync(GetAllBlogPostTagRequestDto request);
    Task<PagedResult<SearchBlogPostTagDto>> SearchAsync(SearchBlogPostTagRequestDto request);
    void Add(BlogPostTag blogPostTag);
    void Remove(BlogPostTag blogPostTag);
    ValueTask<BlogPostTag?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<BlogPostTag?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<BlogPostTag, bool>> expression, CancellationToken cancellationToken = default);
}