using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Tag : BaseEntity
{
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<BlogPostTag> BlogPostTags { get; private set; } = default!;


    public static Tag Create(string title)
        => new()
        {
            Title = title
        };
    public void Update(string title, bool isActive)
    {
        Title = title;
        IsActive = isActive;
    }
}