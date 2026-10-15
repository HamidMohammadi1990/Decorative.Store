namespace Store.Infrastructure.Persistence.SeedData.Cms;

/// <summary>
/// Linux publish uses case-sensitive paths; repo folder is <c>SeedData/Cms</c>.
/// </summary>
internal static class CmsSeedFilePaths
{
    public static string GetJsonPath(string fileName)
    {
        var root = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var preferred = Path.Combine(root, "Cms", fileName);
        if (File.Exists(preferred))
            return preferred;

        var legacy = Path.Combine(root, "cms", fileName);
        return legacy;
    }

    public static bool JsonExists(string fileName)
        => File.Exists(GetJsonPath(fileName));
}
