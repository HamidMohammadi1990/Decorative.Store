using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductFileTranslation : BaseEntity
{
    public int ProductFileId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;

    public ProductFile ProductFile { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static ProductFileTranslation Create(string title, int languageId)
        => new()
        {
            Title = title,
            LanguageId = languageId
        };

    public void Update(string title)
    {
        Title = title;
    }
}
