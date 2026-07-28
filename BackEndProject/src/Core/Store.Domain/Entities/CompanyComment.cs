using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class CompanyComment : BaseEntity
{
    public int? ParentId { get; private set; }
    public int UserId { get; private set; }
    public int CompanyId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Rate { get; private set; }
    public CommentStatusType StatusType { get; private set; } = CommentStatusType.Pending;


    public User User { get; private set; } = default!;
    public Company Company { get; private set; } = default!;
    public CompanyComment Parent { get; private set; } = default!;
    public List<CompanyComment> Children { get; private set; } = default!;


    public static CompanyComment Create(int? parentId, int userId, int companyId, string title, string description, int rate)
        => new()
        {
            Rate = rate,
            Title = title,
            UserId = userId,
            ParentId = parentId,
            CompanyId = companyId,
            Description = description
        };

    public void Update(string title, string description, int rate)
    {
        Rate = rate;
        Title = title;
        Description = description;
    }

    public void ChangeStatus(CommentStatusType status)
    {
        StatusType = status;
    }
}