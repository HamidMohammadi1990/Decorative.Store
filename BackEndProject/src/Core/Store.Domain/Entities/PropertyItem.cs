using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyItem : BaseEntity
{
    public string Code { get; private set; } = default!;
    public int PropertyId { get; private set; }
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Property Property { get; private set; } = default!;
    public ICollection<OrderItemProperty> OrderItemProperties { get; private set; } = default!;
    public ICollection<PropertyItemDependency> ParentPropertyItems { get; private set; } = default!;
    public ICollection<PropertyItemDependency> DependentPropertyItems { get; private set; } = default!;
    public ICollection<ProductProperty> ProductProperties { get; private set; } = default!;
    public ICollection<PropertyItemTranslation> Translations { get; private set; } = [];

    public static PropertyItem Create(string code, int propertyId, int priority)
        => new()
        {
            Code = code,
            PropertyId = propertyId,
            Priority = priority
        };

    public PropertyItemTranslation UpsertTranslation(int languageId, string title)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title);
            return existing;
        }

        var translation = PropertyItemTranslation.Create(title, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(string code, int propertyId, bool isActive, int priority, int languageId, string title)
    {
        Code = code;
        IsActive = isActive;
        PropertyId = propertyId;
        Priority = priority;
        UpsertTranslation(languageId, title);
    }
}
