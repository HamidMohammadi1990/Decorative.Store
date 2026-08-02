namespace Store.Infrastructure.Persistence.SeedData;

internal static class ProductImageSeedAssets
{
    private static readonly string[] SupportedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".svg"];

    public static string EnsureCopiedToUploads(string imageBaseName)
    {
        var normalizedBaseName = imageBaseName.Trim();
        var seedImageDirectory = Path.Combine(AppContext.BaseDirectory, "SeedData", "images");
        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Products");

        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        foreach (var extension in SupportedExtensions)
        {
            var sourcePath = Path.Combine(seedImageDirectory, normalizedBaseName + extension);
            if (!File.Exists(sourcePath))
                continue;

            var fileName = normalizedBaseName + extension;
            var targetPath = Path.Combine(uploadDirectory, fileName);

            if (!File.Exists(targetPath))
                File.Copy(sourcePath, targetPath);

            return fileName;
        }

        throw new InvalidOperationException(
            $"Seed image '{normalizedBaseName}' was not found in '{seedImageDirectory}'.");
    }
}
