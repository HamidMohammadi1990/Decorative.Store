using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostFile : BaseEntity
{
    public int BlogPostId { get; private set; }
    public string FileName { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public bool IsMain { get; private set; }

    public BlogPost BlogPost { get; private set; } = default!;
    public ICollection<BlogPostFileTranslation> Translations { get; private set; } = [];

    public static BlogPostFile Create(int blogPostId, string fileName, bool isMain)
        => new()
        {
            BlogPostId = blogPostId,
            FileName = fileName,
            IsMain = isMain,
        };

    public BlogPostFileTranslation UpsertTranslation(int languageId, string title)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title);
            return existing;
        }

        var translation = BlogPostFileTranslation.Create(title, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetMain(bool isMain)
    {
        IsMain = isMain;
    }
}
