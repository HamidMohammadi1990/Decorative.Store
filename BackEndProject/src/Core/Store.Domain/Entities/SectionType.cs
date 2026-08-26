using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionType : BaseEntity
{
    public bool IsActive { get; private set; } = true;

    public ICollection<Section> Sections { get; private set; } = [];
    public ICollection<SectionTypeTranslation> Translations { get; private set; } = [];

    public static SectionType Create(bool isActive)
        => new() { IsActive = isActive };

    public SectionTypeTranslation UpsertTranslation(int languageId, string name)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(name);
            return existing;
        }

        var translation = SectionTypeTranslation.Create(name, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(bool isActive, int languageId, string name)
    {
        IsActive = isActive;
        UpsertTranslation(languageId, name);
    }
}
