using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductPropertyRuleTranslation : BaseEntity
{
    public int ProductPropertyRuleId { get; private set; }
    public int LanguageId { get; private set; }
    public string? Description { get; private set; }

    public ProductPropertyRule ProductPropertyRule { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static ProductPropertyRuleTranslation Create(string? description, int languageId)
        => new()
        {
            Description = description,
            LanguageId = languageId
        };

    public void Update(string? description)
    {
        Description = description;
    }
}
