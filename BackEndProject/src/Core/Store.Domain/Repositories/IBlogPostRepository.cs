using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostRepository
{
    void Add(BlogPost blogPost);
    ValueTask<BlogPost?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<BlogPost?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<BlogPost, bool>> expression, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllBlogPostDto>> GetAllAsync(GetAllBlogPostRequestDto request);
    Task<PagedResult<SearchBlogPostDto>> SearchAsync(SearchBlogPostRequestDto request);
}