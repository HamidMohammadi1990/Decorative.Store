using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPost : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public int BlogPostCategoryId { get; private set; }
    public string MetaDescription { get; private set; } = default!;
    public string SeoKeywords { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public int UserId { get; private set; }
    public int ReadingTimeInMinutes { get; set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsPublished { get; private set; }


    public User User { get; private set; } = default!;
    public BlogPostCategory BlogPostCategory { get; private set; } = default!;
    public ICollection<BlogPostTag> BlogPostTags { get; private set; } = default!;
    public ICollection<BlogPostLike> BlogPostLikes { get; private set; } = default!;
    public ICollection<BlogPostComment> BlogPostComments { get; private set; } = default!;


    public static BlogPost Create(string title, string slug, int blogPostCategoryId, string metaDescription, string seoKeywords, string content,
                                  int userId, int readingTimeInMinutes)
        => new()
        {
            Slug = slug,
            Title = title,
            UserId = userId,
            Content = content,
            SeoKeywords = seoKeywords,
            MetaDescription = metaDescription,
            BlogPostCategoryId = blogPostCategoryId,
            ReadingTimeInMinutes = readingTimeInMinutes
        };
    public void Update(string title, string slug, int blogPostCategoryId, string metaDescription, string seoKeywords, string content, int readingTimeInMinutes)
    {
        Slug = slug;
        Title = title;
        Content = content;
        SeoKeywords = seoKeywords;
        UpdatedOnUtc = DateTime.UtcNow;
        MetaDescription = metaDescription;
        BlogPostCategoryId = blogPostCategoryId;
        ReadingTimeInMinutes = readingTimeInMinutes;
    }
    public void Publish()
    {
        IsPublished = true;
        PublishedOnUtc = DateTime.UtcNow;
    }
}