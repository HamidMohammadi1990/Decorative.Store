using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyItemTranslation : BaseEntity
{
    public int PropertyItemId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;

    public PropertyItem PropertyItem { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static PropertyItemTranslation Create(string title, int languageId)
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
