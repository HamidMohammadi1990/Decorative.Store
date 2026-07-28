using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CompanyStoryLike : BaseEntity
{
    public int CompanyStoryId { get; private set; }
    public int? UserId { get; private set; }
    public string ClientIP { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public User? User { get; private set; }
    public CompanyStory CompanyStory { get; private set; } = default!;


    public static CompanyStoryLike Create(int companyStoryId, string clientIP, int? userId)
        => new()
        {
            UserId = userId,
            ClientIP = clientIP,
            CompanyStoryId = companyStoryId
        };
}
