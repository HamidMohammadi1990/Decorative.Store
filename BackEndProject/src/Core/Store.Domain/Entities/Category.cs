using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Category : BaseEntity
{
    public string Code { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public ICollection<SubCategory> SubCategories { get; set; } = [];
    public ICollection<CategoryTranslation> Translations { get; private set; } = [];

    public static Category Create(string code)
        => new() { Code = code };

    public CategoryTranslation UpsertTranslation(int languageId, string title, string slug)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug);
            return existing;
        }

        var translation = CategoryTranslation.Create(title, slug, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(string code, bool isActive, int languageId, string title, string slug)
    {
        Code = code;
        IsActive = isActive;
        UpsertTranslation(languageId, title, slug);
    }

    public void AddSubCategories(List<SubCategory> subCategories)
    {
        foreach (var subCategory in subCategories)
            SubCategories.Add(subCategory);
    }
}
