using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionType : BaseEntity
{
    public bool IsActive { get; private set; } = true;
    public string? AdminDescription { get; private set; }

    public ICollection<Section> Sections { get; private set; } = [];
    public ICollection<SectionTypeTranslation> Translations { get; private set; } = [];

    public static SectionType Create(bool isActive, string? adminDescription = null)
        => new()
        {
            IsActive = isActive,
            AdminDescription = NormalizeAdminDescription(adminDescription),
        };

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

    public void Update(bool isActive, int languageId, string name, string? adminDescription = null)
    {
        IsActive = isActive;
        AdminDescription = NormalizeAdminDescription(adminDescription);
        UpsertTranslation(languageId, name);
    }

    private static string? NormalizeAdminDescription(string? adminDescription)
    {
        var trimmed = adminDescription?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }
}
