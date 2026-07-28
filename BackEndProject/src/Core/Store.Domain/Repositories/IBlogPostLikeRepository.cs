using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPostLikes;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostLikeRepository
{
    void Add(BlogPostLike blogPostLike);
    Task<BlogPostLike?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<BlogPostLikeDto>> GetAllAsync(GetAllBlogPostLikeRequestDto request);
    Task<bool> AnyAsync(Expression<Func<BlogPostLike, bool>> expression, CancellationToken cancellationToken = default);
}