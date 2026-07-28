using System.Linq.Expressions;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostCategoryRepository
{
    Task<PagedResult<GetAllBlogPostCategoryResponseDto>> GetAllAsync(GetAllBlogPostCategoryRequestDto request);
    Task<PagedResult<SearchBlogPostCategoryResponseDto>> SearchAsync(SearchBlogPostCategoryRequestDto request);
    ValueTask<BlogPostCategory?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(BlogPostCategory blogPostCategory);
    Task<BlogPostCategory?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<BlogPostCategory, bool>> expression, CancellationToken cancellationToken = default);
}