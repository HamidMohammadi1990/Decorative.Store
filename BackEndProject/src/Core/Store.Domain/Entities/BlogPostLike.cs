using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostLike : BaseEntity
{
    public int? UserId { get; private set; }
    public int BlogPostId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string ClientIP { get; private set; } = default!;


    public User User { get; private set; } = default!;
    public BlogPost BlogPost { get; private set; } = default!;


    public static BlogPostLike Create(int blogPostId, string clientIP, int? userId)
        => new()
        {
            UserId = userId,
            ClientIP = clientIP,
            BlogPostId = blogPostId
        };
}