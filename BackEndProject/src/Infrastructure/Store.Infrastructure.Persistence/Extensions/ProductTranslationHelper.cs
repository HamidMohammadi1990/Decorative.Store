using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Extensions;

internal static class ProductTranslationHelper
{
    public static string ResolveTitle(
        IEnumerable<ProductTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<ProductTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Title
            ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Title
            ?? string.Empty;
    }

    public static string ResolveSlug(
        IEnumerable<ProductTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<ProductTranslation> ?? translations.ToList();
        return list.FirstOrDefault(t => t.LanguageId == languageId)?.Slug
            ?? list.FirstOrDefault(t => t.LanguageId == defaultLanguageId)?.Slug
            ?? string.Empty;
    }
}
