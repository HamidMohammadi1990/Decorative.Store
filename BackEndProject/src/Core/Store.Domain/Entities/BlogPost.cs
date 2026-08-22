using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPost : BaseEntity
{
    public string Code { get; private set; } = default!;
    public int BlogPostCategoryId { get; private set; }
    public int UserId { get; private set; }
    public int ReadingTimeInMinutes { get; set; }
    public bool IsFeatured { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; }
    public DateTime? PublishedOnUtc { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsPublished { get; private set; }

    public User User { get; private set; } = default!;
    public BlogPostCategory BlogPostCategory { get; private set; } = default!;
    public ICollection<BlogPostTag> BlogPostTags { get; private set; } = default!;
    public ICollection<BlogPostLike> BlogPostLikes { get; private set; } = default!;
    public ICollection<BlogPostComment> BlogPostComments { get; private set; } = default!;
    public ICollection<BlogPostTranslation> Translations { get; private set; } = [];

    public static BlogPost Create(
        string code,
        int blogPostCategoryId,
        int userId,
        int readingTimeInMinutes,
        bool isFeatured = false)
        => new()
        {
            Code = code,
            UserId = userId,
            BlogPostCategoryId = blogPostCategoryId,
            ReadingTimeInMinutes = readingTimeInMinutes,
            IsFeatured = isFeatured,
            IsActive = true,
        };

    public BlogPostTranslation UpsertTranslation(
        int languageId,
        string title,
        string slug,
        string metaDescription,
        string seoKeywords,
        string content)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug, metaDescription, seoKeywords, content);
            return existing;
        }

        var translation = BlogPostTranslation.Create(title, slug, metaDescription, seoKeywords, content, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(
        string code,
        int blogPostCategoryId,
        int readingTimeInMinutes,
        bool isFeatured,
        int languageId,
        string title,
        string slug,
        string metaDescription,
        string seoKeywords,
        string content)
    {
        Code = code;
        BlogPostCategoryId = blogPostCategoryId;
        ReadingTimeInMinutes = readingTimeInMinutes;
        IsFeatured = isFeatured;
        UpdatedOnUtc = DateTime.UtcNow;
        UpsertTranslation(languageId, title, slug, metaDescription, seoKeywords, content);
    }

    public void Publish(DateTime? publishedOnUtc = null)
    {
        IsPublished = true;
        PublishedOnUtc = publishedOnUtc ?? DateTime.UtcNow;
    }
}
