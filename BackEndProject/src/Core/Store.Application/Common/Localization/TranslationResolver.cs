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

    public static (string Title, string Slug, string Description) Resolve(
        IEnumerable<ProductTranslation> translations,
        int languageId,
        int defaultLanguageId)
        => ResolveCoreWithDescription(
            translations,
            languageId,
            defaultLanguageId,
            t => t.LanguageId,
            t => t.Title,
            t => t.Slug,
            t => t.Description);

    public static (string Title, string? Description) Resolve(
        IEnumerable<PropertyTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<PropertyTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);

        return (
            current?.Title ?? fallback?.Title ?? string.Empty,
            current?.Description ?? fallback?.Description);
    }

    public static string ResolveTitle(
        IEnumerable<PropertyCategoryTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<PropertyCategoryTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);
        return current?.Title ?? fallback?.Title ?? string.Empty;
    }

    public static string ResolveTitle(
        IEnumerable<PropertyItemTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<PropertyItemTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);
        return current?.Title ?? fallback?.Title ?? string.Empty;
    }

    public static string? ResolveDescription(
        IEnumerable<ProductPropertyRuleTranslation> translations,
        int languageId,
        int defaultLanguageId)
    {
        var list = translations as IList<ProductPropertyRuleTranslation> ?? translations.ToList();
        var current = list.FirstOrDefault(t => t.LanguageId == languageId);
        var fallback = list.FirstOrDefault(t => t.LanguageId == defaultLanguageId);
        return current?.Description ?? fallback?.Description;
    }

    private static (string Title, string Slug, string Description) ResolveCoreWithDescription<T>(
        IEnumerable<T> translations,
        int languageId,
        int defaultLanguageId,
        Func<T, int> languageIdSelector,
        Func<T, string> titleSelector,
        Func<T, string> slugSelector,
        Func<T, string> descriptionSelector)
    {
        var list = translations as IList<T> ?? translations.ToList();
        var current = list.FirstOrDefault(t => languageIdSelector(t) == languageId);
        var fallback = list.FirstOrDefault(t => languageIdSelector(t) == defaultLanguageId);

        return (
            current is not null ? titleSelector(current) : fallback is not null ? titleSelector(fallback) : string.Empty,
            current is not null ? slugSelector(current) : fallback is not null ? slugSelector(fallback) : string.Empty,
            current is not null ? descriptionSelector(current) : fallback is not null ? descriptionSelector(fallback) : string.Empty);
    }

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
