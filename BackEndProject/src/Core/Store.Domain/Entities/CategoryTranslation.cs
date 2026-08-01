using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CategoryTranslation : BaseEntity
{
    public int CategoryId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;

    public Category Category { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static CategoryTranslation Create(string title, string slug, int languageId)
        => new()
        {
            Title = title,
            Slug = slug,
            LanguageId = languageId
        };

    public void Update(string title, string slug)
    {
        Title = title;
        Slug = slug;
    }
}
