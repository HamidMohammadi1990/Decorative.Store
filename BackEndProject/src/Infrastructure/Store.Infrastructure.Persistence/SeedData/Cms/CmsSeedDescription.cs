namespace Store.Infrastructure.Persistence.SeedData.Cms;

internal static class CmsSeedDescription
{
    public static string BuildSlideDescription(string? eyebrow, string? subtitle, string ctaLabel)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(eyebrow))
            parts.Add($"eyebrow:{eyebrow.Trim()}");
        if (!string.IsNullOrWhiteSpace(subtitle))
            parts.Add(subtitle.Trim());
        if (!string.IsNullOrWhiteSpace(ctaLabel))
            parts.Add($"cta:{ctaLabel.Trim()}");

        return string.Join('|', parts);
    }

    public static string BuildTileDescription(string? subtitle, string linkLabel)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(subtitle))
            parts.Add(subtitle.Trim());
        if (!string.IsNullOrWhiteSpace(linkLabel))
            parts.Add($"cta:{linkLabel.Trim()}");

        return string.Join('|', parts);
    }

    public static string BuildFeaturedDescription(string? subtitle, string linkLabel)
    {
        return BuildTileDescription(subtitle, linkLabel);
    }

    public static string BuildAboutHeroDescription(string? eyebrow, string? subtitle, string ctaLabel)
        => BuildSlideDescription(eyebrow, subtitle, ctaLabel);
}
