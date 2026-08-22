using System.Linq.Expressions;
using Store.Domain.Dtos.BlogPostCategories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostCategoryRepository
{
    Task<PagedResult<GetAllBlogPostCategoryResponseDto>> GetAllAsync(
        GetAllBlogPostCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task<PagedResult<SearchBlogPostCategoryResponseDto>> SearchAsync(
        SearchBlogPostCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    ValueTask<BlogPostCategory?> FindAsync(int id, CancellationToken cancellationToken = default);

    Task<BlogPostCategory?> FindWithTranslationsAsync(int id, CancellationToken cancellationToken = default);

    Task<BlogPostCategory?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    void Add(BlogPostCategory blogPostCategory);

    Task<bool> ExistsCodeAsync(string code, int? excludeCategoryId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsTranslationAsync(
        int languageId,
        string title,
        string slug,
        int? excludeCategoryId = null,
        CancellationToken cancellationToken = default);
}
