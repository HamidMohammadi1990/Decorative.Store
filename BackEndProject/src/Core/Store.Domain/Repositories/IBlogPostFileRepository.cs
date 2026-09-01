using Store.Domain.Dtos.BlogPostFiles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IBlogPostFileRepository
{
    void Add(BlogPostFile blogPostFile);
    void Remove(BlogPostFile blogPostFile);
    Task<BlogPostFile?> GetWithTranslationsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<BlogPostFile?> FindAsync(int blogPostFileId, CancellationToken cancellationToken = default);
    Task<BlogPostFile?> GetWithTranslationsAsync(int id, CancellationToken cancellationToken = default);
    Task ClearMainFlagsAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllBlogPostFileResponseDto>> GetAllAsync(
        GetAllBlogPostFileRequestDto request,
        CancellationToken cancellationToken = default);
    Task<List<BlogPostImageDto>> GetActiveImagesByBlogPostIdAsync(
        int blogPostId,
        CancellationToken cancellationToken = default);
    Task<Dictionary<int, string>> GetCoverFileNamesByBlogPostIdsAsync(
        IReadOnlyCollection<int> blogPostIds,
        CancellationToken cancellationToken = default);
}
