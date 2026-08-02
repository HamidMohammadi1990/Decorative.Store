using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyTranslation : BaseEntity
{
    public int PropertyId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }

    public Property Property { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static PropertyTranslation Create(string title, string? description, int languageId)
        => new()
        {
            Title = title,
            Description = description,
            LanguageId = languageId
        };

    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}
