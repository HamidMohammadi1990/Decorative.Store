using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductQuestion : BaseEntity
{
    public int ProductId { get; private set; }
    public int UserId { get; private set; }
    public string Question { get; private set; } = default!;
    public string? Answer { get; private set; }
    public int? AnsweredByUserId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    public Product Product { get; private set; } = default!;
    public User User { get; private set; } = default!;
    public User? AnsweredByUser { get; private set; }

    public static ProductQuestion Create(int userId, int productId, string question)
        => new()
        {
            UserId = userId,
            ProductId = productId,
            Question = question.Trim()
        };

    public void SetAnswer(string answer, int answeredByUserId)
    {
        Answer = answer.Trim();
        AnsweredByUserId = answeredByUserId;
    }
}
