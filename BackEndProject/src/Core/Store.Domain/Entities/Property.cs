using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Property : BaseEntity
{
    public int? ParentId { get; private set; }
    public string Code { get; private set; } = default!;
    public int PropertyCategoryId { get; private set; }
    public int Priority { get; private set; }
    public PropertyType PropertyType { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Property Parent { get; private set; } = default!;
    public List<Property> Children { get; private set; } = default!;
    public PropertyCategory PropertyCategory { get; private set; } = default!;
    public ICollection<PropertyItem> PropertyItems { get; private set; } = default!;
    public ICollection<ProductProperty> ProductProperties { get; private set; } = default!;
    public ICollection<OrderItemProperty> OrderItemProperties { get; private set; } = default!;
    public ICollection<PropertyTranslation> Translations { get; private set; } = [];

    public static Property Create(
        PropertyType propertyType,
        int? parentId,
        string code,
        int propertyCategoryId,
        int priority)
        => new()
        {
            Code = code,
            ParentId = parentId,
            Priority = priority,
            PropertyType = propertyType,
            PropertyCategoryId = propertyCategoryId,
        };

    public PropertyTranslation UpsertTranslation(int languageId, string title, string? description)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, description);
            return existing;
        }

        var translation = PropertyTranslation.Create(title, description, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void DeActive()
    {
        IsActive = false;
    }

    public void Update(
        PropertyType propertyType,
        int? parentId,
        string code,
        int propertyCategoryId,
        int priority,
        bool isActive,
        int languageId,
        string title,
        string? description)
    {
        Code = code;
        IsActive = isActive;
        ParentId = parentId;
        Priority = priority;
        PropertyType = propertyType;
        PropertyCategoryId = propertyCategoryId;
        UpsertTranslation(languageId, title, description);
    }
}
