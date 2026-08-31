using System.Linq.Expressions;
using Store.Domain.Dtos.UserStoryComments;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserStoryCommentRepository
{
    Task<PagedResult<GetAllUserStoryCommentResponseDto>> GetAllAsync(GetAllUserStoryCommentRequestDto request);
    Task<PagedResult<SearchUserStoryCommentResponseDto>> SearchAsync(SearchUserStoryCommentRequestDto request);
    ValueTask<UserStoryComment?> FindAsync(int id, CancellationToken cancellationToken = default);
    void Add(UserStoryComment userStoryComment);
    Task<UserStoryComment?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<UserStoryComment, bool>> expression, CancellationToken cancellationToken = default);
    Task<Dictionary<int, int>> GetApprovedCommentCountsByStoryIdsAsync(IReadOnlyCollection<int> storyIds, CancellationToken cancellationToken = default);
    Task DeleteByStoryIdAsync(int userStoryId, CancellationToken cancellationToken = default);
}
