using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionItemTranslation : BaseEntity
{
    public int SectionItemId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? Url { get; private set; }

    public SectionItem SectionItem { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static SectionItemTranslation Create(
        string title,
        int languageId,
        string? description = null,
        string? url = null)
        => new()
        {
            Title = title,
            LanguageId = languageId,
            Description = description,
            Url = url,
        };

    public void Update(string title, string? description = null, string? url = null)
    {
        Title = title;
        Description = description;
        Url = url;
    }
}
