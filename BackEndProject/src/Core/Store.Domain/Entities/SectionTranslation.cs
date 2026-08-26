using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionTranslation : BaseEntity
{
    public int SectionId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string Url { get; private set; } = default!;

    public Section Section { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static SectionTranslation Create(
        string title,
        string url,
        int languageId,
        string? description = null)
        => new()
        {
            Title = title,
            Url = url,
            LanguageId = languageId,
            Description = description,
        };

    public void Update(string title, string url, string? description = null)
    {
        Title = title;
        Url = url;
        Description = description;
    }
}
