using System.Linq.Expressions;
using Store.Domain.Dtos.ProductComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductCommentRepository
{
    void Add(ProductComment productComment);
    void Remove(ProductComment productComment);
    Task<ProductComment?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<ProductComment?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProductCommentResponseDto>> GetAllAsync(GetAllProductCommentRequestDto request, CancellationToken cancellationToken = default);
    Task<PagedResult<SearchProductCommentResponseDto>> SearchAsync(SearchProductCommentRequestDto request, CancellationToken cancellationToken = default);
    Task<List<GetMyProductCommentDto>> GetByUserIdAsync(int userId, int languageId, int defaultLanguageId, CancellationToken cancellationToken = default);
    Task<ProductCommentReaction?> FindReactionAsync(int userId, int productCommentId, CancellationToken cancellationToken = default);
    void AddReaction(ProductCommentReaction reaction);
    void RemoveReaction(ProductCommentReaction reaction);
    Task<(int HelpfulCount, int NotHelpfulCount)> GetReactionCountsAsync(
        int productCommentId,
        CancellationToken cancellationToken = default);
}