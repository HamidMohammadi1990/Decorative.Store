using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CompanyStoryComment : BaseEntity
{
    public int? ParentId { get; private set; }
    public int CompanyStoryId { get; private set; }
    public int CreatedByUserId { get; private set; }
    public int? ApprovedByUserId { get; private set; }
    public string Content { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? ApprovedOnUtc { get; private set; }
    public bool IsApproved { get; private set; }


    public CompanyStory CompanyStory { get; private set; } = default!;
    public User CreatedByUser { get; private set; } = default!;
    public User? ApprovedByUser { get; private set; }
    public CompanyStoryComment? Parent { get; private set; }
    public ICollection<CompanyStoryComment> Children { get; private set; } = [];


    public static CompanyStoryComment Create(
        int? parentId,
        int companyStoryId,
        int createdByUserId,
        string content)
        => new()
        {
            Content = content,
            ParentId = parentId,
            CompanyStoryId = companyStoryId,
            CreatedByUserId = createdByUserId
        };

    public void Update(int? parentId, string content)
    {
        Content = content;
        ParentId = parentId;
    }

    public void Approve(int approvedByUserId)
    {
        IsApproved = true;
        ApprovedOnUtc = DateTime.UtcNow;
        ApprovedByUserId = approvedByUserId;
    }
}
