using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserStoryLikeRepository
{
    void Add(UserStoryLike userStoryLike);
    void Remove(UserStoryLike userStoryLike);
    Task<UserStoryLike?> FindByUserAndStoryAsync(int userId, int userStoryId, CancellationToken cancellationToken = default);
    Task<Dictionary<int, int>> GetLikeCountsByStoryIdsAsync(IReadOnlyCollection<int> storyIds, CancellationToken cancellationToken = default);
    Task<HashSet<int>> GetLikedStoryIdsForUserAsync(int userId, IReadOnlyCollection<int> storyIds, CancellationToken cancellationToken = default);
    Task DeleteByStoryIdAsync(int userStoryId, CancellationToken cancellationToken = default);
}
