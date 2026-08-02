using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductFile : BaseEntity
{
    public int ProductId { get; private set; }
    public string FileName { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public bool IsMain { get; private set; }

    public Product Product { get; private set; } = default!;
    public ICollection<ProductFileTranslation> Translations { get; private set; } = [];

    public static ProductFile Create(int productId, string fileName, bool isMain)
        => new()
        {
            ProductId = productId,
            FileName = fileName,
            IsMain = isMain
        };

    public ProductFileTranslation UpsertTranslation(int languageId, string title)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title);
            return existing;
        }

        var translation = ProductFileTranslation.Create(title, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }
}
