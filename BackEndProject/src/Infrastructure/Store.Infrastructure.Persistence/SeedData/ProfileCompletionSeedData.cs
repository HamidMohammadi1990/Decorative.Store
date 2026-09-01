using System.Text.Json;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class ProfileCompletionSeedData
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static string ConfigJson { get; } = LoadConfigJson();

    private static string LoadConfigJson()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "profile-completion.config.json");
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException($"Profile completion seed file was not found at '{jsonPath}'.");

        return File.ReadAllText(jsonPath);
    }
}
