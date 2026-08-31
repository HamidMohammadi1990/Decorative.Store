using Store.Domain.Common;

namespace Store.Domain.Entities;

public class UserStoryLike : BaseEntity
{
    public int UserId { get; private set; }
    public int UserStoryId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = default!;
    public UserStory UserStory { get; private set; } = default!;

    public static UserStoryLike Create(int userId, int userStoryId)
        => new()
        {
            UserId = userId,
            UserStoryId = userStoryId,
        };
}
