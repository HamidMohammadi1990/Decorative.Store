namespace Store.Common.Localization;

public static class LanguageCultureNormalizer
{
    public static string Normalize(string? culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            return "fa-IR";

        var normalized = culture.Trim().ToLowerInvariant();
        return normalized switch
        {
            "fa" or "fa-ir" => "fa-IR",
            "en" or "en-us" => "en-US",
            _ when normalized.StartsWith("fa", StringComparison.Ordinal) => "fa-IR",
            _ when normalized.StartsWith("en", StringComparison.Ordinal) => "en-US",
            _ => culture.Trim()
        };
    }

    public static string? MapAcceptLanguageSegment(string? languageSegment)
    {
        if (string.IsNullOrWhiteSpace(languageSegment))
            return null;

        var language = languageSegment.Split(';')[0].Trim();
        var normalized = language.ToLowerInvariant();

        if (normalized.StartsWith("fa", StringComparison.Ordinal))
            return "fa-IR";

        if (normalized.StartsWith("en", StringComparison.Ordinal))
            return "en-US";

        return Normalize(language);
    }
}
