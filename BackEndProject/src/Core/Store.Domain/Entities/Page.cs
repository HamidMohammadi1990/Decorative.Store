using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Page : BaseEntity
{
    public PageType Type { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? AdminDescription { get; private set; }

    public ICollection<PageSection> PageSections { get; private set; } = [];
    public ICollection<PageTranslation> Translations { get; private set; } = [];

    public static Page Create(PageType type, bool isActive, string? adminDescription = null)
        => new()
        {
            Type = type,
            IsActive = isActive,
            AdminDescription = NormalizeAdminDescription(adminDescription),
        };

    public PageTranslation UpsertTranslation(
        int languageId,
        string title,
        string slug,
        string? metaTitle = null,
        string? metaDescription = null)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug, metaTitle, metaDescription);
            return existing;
        }

        var translation = PageTranslation.Create(title, slug, languageId, metaTitle, metaDescription);
        Translations.Add(translation);
        return translation;
    }

    public void Update(
        PageType type,
        bool isActive,
        int languageId,
        string title,
        string slug,
        string? metaTitle,
        string? metaDescription,
        string? adminDescription = null)
    {
        Type = type;
        IsActive = isActive;
        AdminDescription = NormalizeAdminDescription(adminDescription);
        UpsertTranslation(languageId, title, slug, metaTitle, metaDescription);
    }

    private static string? NormalizeAdminDescription(string? adminDescription)
    {
        var trimmed = adminDescription?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }
}
