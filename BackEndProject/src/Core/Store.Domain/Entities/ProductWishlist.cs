using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductWishlist : BaseEntity
{
    public int UserId { get; private set; }
    public int ProductId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = default!;
    public Product Product { get; private set; } = default!;

    public static ProductWishlist Create(int userId, int productId)
        => new()
        {
            UserId = userId,
            ProductId = productId,
        };
}
