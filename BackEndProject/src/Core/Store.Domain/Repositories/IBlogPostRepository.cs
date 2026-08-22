using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.BlogPosts;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostRepository
{
    void Add(BlogPost blogPost);

    ValueTask<BlogPost?> FindAsync(int id, CancellationToken cancellationToken = default);

    Task<BlogPost?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);

    Task<BlogPost?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<BlogPost, bool>> expression, CancellationToken cancellationToken = default);

    Task<bool> ExistsCodeAsync(string code, int? excludeBlogPostId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeBlogPostId = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<GetAllBlogPostDto>> GetAllAsync(GetAllBlogPostRequestDto request);

    Task<PagedResult<SearchBlogPostDto>> SearchAsync(SearchBlogPostRequestDto request);

    Task<BlogPostDetailDto?> GetDetailBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
