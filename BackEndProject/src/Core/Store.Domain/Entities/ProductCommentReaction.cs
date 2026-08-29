using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductCommentReaction : BaseEntity
{
    public int ProductCommentId { get; private set; }
    public int UserId { get; private set; }
    public bool IsHelpful { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public ProductComment ProductComment { get; private set; } = default!;
    public User User { get; private set; } = default!;

    public static ProductCommentReaction Create(int productCommentId, int userId, bool isHelpful)
        => new()
        {
            ProductCommentId = productCommentId,
            UserId = userId,
            IsHelpful = isHelpful,
            CreatedOnUtc = DateTime.UtcNow,
        };

    public void SetHelpful(bool isHelpful) => IsHelpful = isHelpful;
}
