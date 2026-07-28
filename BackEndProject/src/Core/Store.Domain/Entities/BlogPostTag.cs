using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostTag : BaseEntity
{
    public int TagId { get; private set; }
    public int BlogPostId { get; private set; }


    public Tag Tag { get; private set; } = default!;
    public BlogPost BlogPost { get; private set; } = default!;


    public static BlogPostTag Create(int tagId, int blogPostid)
        => new()
        {
            TagId = tagId,
            BlogPostId = blogPostid
        };
    public void Update(int tagId, int blogPostid)
    {
        TagId = tagId;
        BlogPostId = blogPostid;
    }
}