using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PageTranslation : BaseEntity
{
    public int PageId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }

    public Page Page { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static PageTranslation Create(
        string title,
        string slug,
        int languageId,
        string? metaTitle = null,
        string? metaDescription = null)
        => new()
        {
            Title = title,
            Slug = slug.Trim(),
            LanguageId = languageId,
            MetaTitle = metaTitle,
            MetaDescription = metaDescription,
        };

    public void Update(
        string title,
        string slug,
        string? metaTitle = null,
        string? metaDescription = null)
    {
        Title = title;
        Slug = slug.Trim();
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
    }
}
