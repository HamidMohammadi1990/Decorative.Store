using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyCategoryTranslation : BaseEntity
{
    public int PropertyCategoryId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;

    public PropertyCategory PropertyCategory { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static PropertyCategoryTranslation Create(string title, int languageId)
        => new()
        {
            Title = title,
            LanguageId = languageId
        };

    public void Update(string title)
    {
        Title = title;
    }
}
