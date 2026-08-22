using Store.Domain.Common;

namespace Store.Domain.Entities;

public class BlogPostCategory : BaseEntity
{
    public string Code { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public ICollection<BlogPost> BlogPosts { get; private set; } = default!;
    public ICollection<BlogPostCategoryTranslation> Translations { get; private set; } = [];

    public static BlogPostCategory Create(string code)
        => new() { Code = code };

    public BlogPostCategoryTranslation UpsertTranslation(int languageId, string title, string slug)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, slug);
            return existing;
        }

        var translation = BlogPostCategoryTranslation.Create(title, slug, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(string code, bool isActive, int languageId, string title, string slug)
    {
        Code = code;
        IsActive = isActive;
        UpsertTranslation(languageId, title, slug);
    }
}
