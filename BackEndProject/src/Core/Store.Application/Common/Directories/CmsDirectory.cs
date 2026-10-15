namespace Edition.Application.Common.Directories;

public static class CmsDirectory
{
    public static string CmsImage = "wwwroot/Uploads/Cms";

    public static string GetImageUrl(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
            return string.Empty;

        var normalized = PublicMediaUrl.NormalizeStoredPath(imageName);
        if (string.IsNullOrWhiteSpace(normalized))
            return string.Empty;

        if (normalized.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
            return normalized;

        if (normalized.StartsWith("/Uploads/", StringComparison.OrdinalIgnoreCase))
            return normalized;

        if (normalized.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || normalized.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return normalized;

        return $"{CmsImage.Replace("wwwroot", "")}/{normalized}";
    }

    /// <summary>
    /// Normalizes DB values (filename, /Uploads/Cms/..., or /images/...) for public API responses.
    /// </summary>
    public static string? ResolvePublicImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;

        var resolved = GetImageUrl(imageUrl);
        return string.IsNullOrWhiteSpace(resolved) ? null : resolved;
    }
}
