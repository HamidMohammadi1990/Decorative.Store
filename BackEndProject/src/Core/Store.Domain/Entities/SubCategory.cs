using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SubCategory : BaseEntity
{
    public string Code { get; private set; } = default!;
    public int CategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Category Category { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];
    public ICollection<Discount> Discounts { get; private set; } = [];
    public ICollection<SubCategoryTranslation> Translations { get; private set; } = [];

    public static SubCategory Create(string code, int categoryId)
        => new()
        {
            Code = code,
            CategoryId = categoryId
        };

    public SubCategoryTranslation UpsertTranslation(int languageId, string title, string slug)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug);
            return existing;
        }

        var translation = SubCategoryTranslation.Create(title, slug, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void AddProducts(List<Product> products)
    {
        foreach (var product in products)
            Products.Add(product);
    }

    public void Update(string code, int categoryId, bool isActive, int languageId, string title, string slug)
    {
        Code = code;
        CategoryId = categoryId;
        IsActive = isActive;
        UpsertTranslation(languageId, title, slug);
    }
}
