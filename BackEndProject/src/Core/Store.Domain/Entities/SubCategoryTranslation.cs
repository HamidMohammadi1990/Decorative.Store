using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SubCategoryTranslation : BaseEntity
{
    public int SubCategoryId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;

    public SubCategory SubCategory { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static SubCategoryTranslation Create(string title, string slug, int languageId)
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
