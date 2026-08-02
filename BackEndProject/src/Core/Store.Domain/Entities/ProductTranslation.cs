using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductTranslation : BaseEntity
{
    public int ProductId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Description { get; private set; } = default!;

    public Product Product { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static ProductTranslation Create(string title, string slug, string description, int languageId)
        => new()
        {
            Title = title,
            Slug = slug,
            Description = description,
            LanguageId = languageId
        };

    public void Update(string title, string slug, string description)
    {
        Title = title;
        Slug = slug;
        Description = description;
    }
}
