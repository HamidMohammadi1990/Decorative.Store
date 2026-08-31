using Store.Domain.Common;

namespace Store.Domain.Entities;

public class UserStoryComment : BaseEntity
{
    public int UserId { get; private set; }
    public int UserStoryId { get; private set; }
    public string Content { get; private set; } = default!;
    public int? ApprovedByUserId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? ApprovedOnUtc { get; private set; }
    public bool IsApproved { get; private set; }

    public User User { get; private set; } = default!;
    public UserStory UserStory { get; private set; } = default!;
    public User? ApprovedByUser { get; private set; }

    public static UserStoryComment Create(int userId, int userStoryId, string content)
        => new()
        {
            UserId = userId,
            UserStoryId = userStoryId,
            Content = content,
        };

    public void Approve(int approvedByUserId)
    {
        IsApproved = true;
        ApprovedOnUtc = DateTime.UtcNow;
        ApprovedByUserId = approvedByUserId;
    }
}
