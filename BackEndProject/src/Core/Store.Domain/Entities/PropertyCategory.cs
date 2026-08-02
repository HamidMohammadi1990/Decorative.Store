using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyCategory : BaseEntity
{
    public string Code { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public ICollection<Property> Properties { get; private set; } = default!;
    public ICollection<PropertyCategoryTranslation> Translations { get; private set; } = [];

    public static PropertyCategory Create(string code)
        => new() { Code = code };

    public PropertyCategoryTranslation UpsertTranslation(int languageId, string title)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title);
            return existing;
        }

        var translation = PropertyCategoryTranslation.Create(title, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(string code, bool isActive, int languageId, string title)
    {
        Code = code;
        IsActive = isActive;
        UpsertTranslation(languageId, title);
    }

    public void DeActive()
    {
        IsActive = false;
    }
}
