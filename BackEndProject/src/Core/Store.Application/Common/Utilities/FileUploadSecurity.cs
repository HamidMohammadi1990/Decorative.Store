using System.Text.RegularExpressions;

namespace Edition.Application.Common.Utilities;

public static partial class FileUploadSecurity
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".zip"
    };

    public static string CreateSafeFileName(string? originalFileName, string? fallbackExtension = null)
    {
        var extension = Path.GetExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(extension))
            extension = fallbackExtension ?? string.Empty;

        extension = extension.ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            extension = fallbackExtension ?? ".bin";

        return $"{Guid.NewGuid():N}{extension}";
    }

    public static string ResolveSafeFilePath(string rootDirectory, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var sanitizedFileName = Path.GetFileName(SafeFileNameRegex().Replace(fileName, string.Empty));
        if (string.IsNullOrWhiteSpace(sanitizedFileName))
            throw new InvalidOperationException("Invalid file name.");

        var fullRoot = Path.GetFullPath(rootDirectory);
        var fullPath = Path.GetFullPath(Path.Combine(fullRoot, sanitizedFileName));

        if (!fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Invalid file path.");

        return fullPath;
    }

    [GeneratedRegex(@"[^a-zA-Z0-9._-]")]
    private static partial Regex SafeFileNameRegex();
}
