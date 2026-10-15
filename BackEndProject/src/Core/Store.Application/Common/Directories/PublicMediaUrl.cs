namespace Edition.Application.Common.Directories;

/// <summary>
/// Normalizes stored media values (filename, /Uploads/..., or absolute dev URLs) for public API responses.
/// </summary>
public static class PublicMediaUrl
{
    public static string? NormalizeStoredPath(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;

        var trimmed = imageUrl.Trim();

        if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            if (Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
                && uri.AbsolutePath.StartsWith("/Uploads/", StringComparison.OrdinalIgnoreCase))
            {
                return uri.AbsolutePath;
            }

            return trimmed;
        }

        if (trimmed.StartsWith('/'))
            return trimmed;

        return trimmed;
    }
}
