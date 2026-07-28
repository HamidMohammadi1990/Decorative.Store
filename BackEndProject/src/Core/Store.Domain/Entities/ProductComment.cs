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
    public int CompanyId { get; private set; }
    public bool IsActive { get; private set; }


    public User User { get; private set; } = default!;
    public Company Company { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public CommentTopic CommentTopic { get; private set; } = default!;


    public static ProductComment Create(int userId, int productId, int companyId, int commentRate, int qualityRating,
                                        int commentTopicId, string description, int affordableRating)
        => new()
        {
            UserId = userId,
            CompanyId = companyId,
            ProductId = productId,
            CommentRate = commentRate,
            Description = description,
            QualityRating = qualityRating,
            CommentTopicId = commentTopicId,
            AffordableRating = affordableRating
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