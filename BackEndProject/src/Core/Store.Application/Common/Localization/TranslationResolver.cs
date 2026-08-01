using Store.Domain.Entities;

namespace Edition.Application.Common.Localization;

public static class TranslationResolver
{
    public static (string Title, string Slug) Resolve(
        IEnumerable<CategoryTranslation> translations,
        int languageId,
        int defaultLanguageId)
        => ResolveCore(translations, languageId, defaultLanguageId, t => t.LanguageId, t => t.Title, t => t.Slug);

    public static (string Title, string Slug) Resolve(
        IEnumerable<SubCategoryTranslation> translations,
        int languageId,
        int defaultLanguageId)
        => ResolveCore(translations, languageId, defaultLanguageId, t => t.LanguageId, t => t.Title, t => t.Slug);

    private static (string Title, string Slug) ResolveCore<T>(
        IEnumerable<T> translations,
        int languageId,
        int defaultLanguageId,
        Func<T, int> languageIdSelector,
        Func<T, string> titleSelector,
        Func<T, string> slugSelector)
    {
        var list = translations as IList<T> ?? translations.ToList();
        var current = list.FirstOrDefault(t => languageIdSelector(t) == languageId);
        var fallback = list.FirstOrDefault(t => languageIdSelector(t) == defaultLanguageId);

        return (
            current is not null ? titleSelector(current) : fallback is not null ? titleSelector(fallback) : string.Empty,
            current is not null ? slugSelector(current) : fallback is not null ? slugSelector(fallback) : string.Empty);
    }
}
