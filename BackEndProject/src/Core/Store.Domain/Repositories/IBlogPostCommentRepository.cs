using System.Linq.Expressions;
using Store.Domain.Dtos.BlogPostComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostCommentRepository
{
    Task<PagedResult<GetAllBlogPostCommentResponseDto>> GetAllAsync(GetAllBlogPostCommentRequestDto request);
    Task<PagedResult<SearchBlogPostCommentResponseDto>> SearchAsync(SearchBlogPostCommentRequestDto request);
    ValueTask<BlogPostComment?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(BlogPostComment blogPostComment);
    void Remove(BlogPostComment blogPostComment);
    Task<BlogPostComment?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<BlogPostComment, bool>> expression, CancellationToken cancellationToken = default);
}