using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CommentTopics;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface ICommentTopicRepository
{
    Task<PagedResult<GetAllCommentTopicResponseDto>> GetAllAsync(GetAllCommentTopicRequestDto request);
    Task<PagedResult<SearchCommentTopicResponseDto>> SearchAsync(SearchCommentTopicRequestDto request);
    Task<CommentTopic?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<CommentTopic?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(CommentTopic commentTopic);
    void Remove(CommentTopic commentTopic);
    Task<bool> AnyAsync(Expression<Func<CommentTopic, bool>> expression, CancellationToken cancellationToken = default);
}