using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProfileCompletionUserState : BaseEntity
{
    public int UserId { get; private set; }
    public string AnswersJson { get; private set; } = "{}";
    public DateTime? RewardClaimedOnUtc { get; private set; }
    public DateTime UpdatedOnUtc { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = default!;

    public static ProfileCompletionUserState Create(int userId, string answersJson)
        => new()
        {
            UserId = userId,
            AnswersJson = answersJson,
            UpdatedOnUtc = DateTime.UtcNow,
        };

    public void UpdateAnswers(string answersJson)
    {
        AnswersJson = answersJson;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ClaimReward()
    {
        RewardClaimedOnUtc = DateTime.UtcNow;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ClearRewardClaim()
    {
        RewardClaimedOnUtc = null;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}
