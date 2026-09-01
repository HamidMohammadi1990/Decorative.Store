using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostFileTranslation : BaseEntity
{
    public int BlogPostFileId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;

    public BlogPostFile BlogPostFile { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static BlogPostFileTranslation Create(string title, int languageId)
        => new()
        {
            Title = title,
            LanguageId = languageId,
        };

    public void Update(string title)
    {
        Title = title;
    }
}
