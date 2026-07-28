using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostCategory : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<BlogPost> BlogPosts { get; private set; } = default!;


    public static BlogPostCategory Create(string title, string slug)
        => new()
        {
            Title = title,
            Slug = slug
        };
    public void Update(string title, string slug, bool isActive)
    {
        Slug = slug;
        Title = title;
        IsActive = isActive;
    }
}