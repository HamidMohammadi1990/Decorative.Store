using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserStoryLikeRepository
    (EditionDbContext context)
    : Repository<UserStoryLike>(context), IUserStoryLikeRepository
{
    public Task<UserStoryLike?> FindByUserAndStoryAsync(
        int userId,
        int userStoryId,
        CancellationToken cancellationToken = default)
        => Context.UserStoryLike
            .FirstOrDefaultAsync(x => x.UserId == userId && x.UserStoryId == userStoryId, cancellationToken);

    public async Task<Dictionary<int, int>> GetLikeCountsByStoryIdsAsync(
        IReadOnlyCollection<int> storyIds,
        CancellationToken cancellationToken = default)
    {
        if (storyIds.Count == 0)
            return new Dictionary<int, int>();

        var counts = await Context.UserStoryLike
            .AsNoTracking()
            .Where(x => storyIds.Contains(x.UserStoryId))
            .GroupBy(x => x.UserStoryId)
            .Select(g => new { UserStoryId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(x => x.UserStoryId, x => x.Count);
    }

    public async Task<HashSet<int>> GetLikedStoryIdsForUserAsync(
        int userId,
        IReadOnlyCollection<int> storyIds,
        CancellationToken cancellationToken = default)
    {
        if (storyIds.Count == 0)
            return [];

        var likedIds = await Context.UserStoryLike
            .AsNoTracking()
            .Where(x => x.UserId == userId && storyIds.Contains(x.UserStoryId))
            .Select(x => x.UserStoryId)
            .ToListAsync(cancellationToken);

        return likedIds.ToHashSet();
    }

    public Task DeleteByStoryIdAsync(int userStoryId, CancellationToken cancellationToken = default)
        => Context.UserStoryLike
            .Where(x => x.UserStoryId == userStoryId)
            .ExecuteDeleteAsync(cancellationToken);
}
