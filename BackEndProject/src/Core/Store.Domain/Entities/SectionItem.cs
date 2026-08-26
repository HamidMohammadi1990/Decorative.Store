using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionItem : BaseEntity
{
    public int SectionId { get; private set; }
    public int Priority { get; private set; }
    public string? Icon { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Section Section { get; private set; } = default!;
    public ICollection<SectionItemTranslation> Translations { get; private set; } = [];

    public static SectionItem Create(
        int sectionId,
        int priority,
        string? icon,
        string? imageUrl,
        bool isActive)
        => new()
        {
            SectionId = sectionId,
            Priority = priority,
            Icon = icon,
            ImageUrl = imageUrl,
            IsActive = isActive,
        };

    public SectionItemTranslation UpsertTranslation(
        int languageId,
        string title,
        string? description = null,
        string? url = null)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, description, url);
            return existing;
        }

        var translation = SectionItemTranslation.Create(title, languageId, description, url);
        Translations.Add(translation);
        return translation;
    }

    public void Update(
        int sectionId,
        int priority,
        string? icon,
        string? imageUrl,
        bool isActive,
        int languageId,
        string title,
        string? description,
        string? url)
    {
        SectionId = sectionId;
        Priority = priority;
        Icon = icon;
        ImageUrl = imageUrl;
        IsActive = isActive;
        UpsertTranslation(languageId, title, description, url);
    }
}
