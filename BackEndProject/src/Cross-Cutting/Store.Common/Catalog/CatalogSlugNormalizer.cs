using System.Text;
using Store.Common.Extensions;

namespace Store.Common.Catalog;

public static class CatalogSlugNormalizer
{
    public static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim().Trim('/').FixPersianChars();
        return trimmed.Normalize(NormalizationForm.FormC).ToLowerInvariant();
    }

    public static string NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;

        var decoded = Uri.UnescapeDataString(path);
        return Normalize(decoded);
    }
}
