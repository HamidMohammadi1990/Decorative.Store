using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostTranslation : BaseEntity
{
    public int BlogPostId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string MetaDescription { get; private set; } = default!;
    public string SeoKeywords { get; private set; } = default!;
    public string Content { get; private set; } = default!;

    public BlogPost BlogPost { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static BlogPostTranslation Create(
        string title,
        string slug,
        string metaDescription,
        string seoKeywords,
        string content,
        int languageId)
        => new()
        {
            Title = title,
            Slug = slug,
            MetaDescription = metaDescription,
            SeoKeywords = seoKeywords,
            Content = content,
            LanguageId = languageId,
        };

    public void Update(
        string title,
        string slug,
        string metaDescription,
        string seoKeywords,
        string content)
    {
        Title = title;
        Slug = slug;
        MetaDescription = metaDescription;
        SeoKeywords = seoKeywords;
        Content = content;
    }
}
