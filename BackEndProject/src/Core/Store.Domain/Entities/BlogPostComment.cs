using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostComment : BaseEntity
{
    public int? ParentId { get; private set; }
    public string Content { get; private set; } = default!;
    public int CreatedByUserId { get; private set; }
    public int? ApprovedByUserId { get; private set; }
    public int BlogPostId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? ApprovedOnUtc { get; private set; }
    public bool IsApproved { get; private set; }


    public BlogPost BlogPost { get; private set; } = default!;
    public User CreatedByUser { get; private set; } = default!;
    public User ApprovedByUser { get; private set; } = default!;
    public BlogPostComment Parent { get; private set; } = default!;
    public ICollection<BlogPostComment> Children { get; private set; } = default!;


    public static BlogPostComment Create(int? parentId, string content, int createdByUserId, int blogPostId)
        => new()
        {
            Content = content,
            ParentId = parentId,
            BlogPostId = blogPostId,
            CreatedByUserId = createdByUserId
        };
    public void Update(int? parentId, string content, int blogPostId)
    {
        Content = content;
        ParentId = parentId;
        BlogPostId = blogPostId;
    }
    public void Approve(int approvedByUserId)
    {
        IsApproved = true;
        ApprovedOnUtc = DateTime.UtcNow;
        ApprovedByUserId = approvedByUserId;
    }
}