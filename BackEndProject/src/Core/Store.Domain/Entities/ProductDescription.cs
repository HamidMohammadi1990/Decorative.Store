using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductDescription : BaseEntity
{
    public string Description { get; private set; } = default!;
    public int ProductId { get; private set; }
    public int LanguageId { get; private set; }

    public Product Product { get; private set; } = default!;
    public Language Language { get; private set; } = default!;

    public static ProductDescription Create(string description, int productId, int languageId)
        => new()
        {
            ProductId = productId,
            Description = description,
            LanguageId = languageId
        };

    public void Update(string description, int languageId)
    {
        Description = description;
        LanguageId = languageId;
    }
}
