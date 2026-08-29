using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductComment : BaseEntity
{
    public int UserId { get; private set; }
    public int CommentTopicId { get; private set; }
    public int CommentRate { get; private set; }
    public string Description { get; private set; } = default!;
    public int QualityRating { get; private set; }
    public int AffordableRating { get; private set; }
    public int ProductId { get; private set; }
    public int? ParentId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; }

    public User User { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public CommentTopic CommentTopic { get; private set; } = default!;
    public ProductComment? Parent { get; private set; }
    public ICollection<ProductComment> Replies { get; private set; } = [];
    public ICollection<ProductCommentReaction> Reactions { get; private set; } = [];

    public static ProductComment Create(
        int userId,
        int productId,
        int commentRate,
        int qualityRating,
        int commentTopicId,
        string description,
        int affordableRating,
        int? parentId = null,
        bool? isActive = null)
        => new()
        {
            UserId = userId,
            ProductId = productId,
            CommentRate = commentRate,
            Description = description,
            QualityRating = qualityRating,
            CommentTopicId = commentTopicId,
            AffordableRating = affordableRating,
            ParentId = parentId,
            CreatedOnUtc = DateTime.UtcNow,
            IsActive = isActive ?? false,
        };

    public void Update(int commentRate, int qualityRating, int commentTopicId, string description, int affordableRating)
    {
        CommentRate = commentRate;
        Description = description;
        QualityRating = qualityRating;
        CommentTopicId = commentTopicId;
        AffordableRating = affordableRating;
    }

    public void Active()
    {
        IsActive = true;
    }

    public void InActive()
    {
        IsActive = false;
    }
}
