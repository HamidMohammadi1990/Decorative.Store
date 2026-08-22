using System.Text.Json;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class BlogPostCategorySeedData
{
    private const string FaFileName = "blog.fa.json";
    private const string EnFileName = "blog.en.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static IReadOnlyList<BlogPostCategorySeedItem> Items { get; } = Build();

    private static IReadOnlyList<BlogPostCategorySeedItem> Build()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var faJson = File.ReadAllText(Path.Combine(seedDataPath, FaFileName));
        var enJson = File.ReadAllText(Path.Combine(seedDataPath, EnFileName));

        var faBlog = JsonSerializer.Deserialize<BlogSeedRoot>(faJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {FaFileName}.");
        var enBlog = JsonSerializer.Deserialize<BlogSeedRoot>(enJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {EnFileName}.");

        var faCategories = faBlog.Categories.ToDictionary(x => x.Slug, StringComparer.OrdinalIgnoreCase);
        var enCategories = enBlog.Categories.ToDictionary(x => x.Slug, StringComparer.OrdinalIgnoreCase);

        var items = new List<BlogPostCategorySeedItem>();

        foreach (var (slug, enCategory) in enCategories)
        {
            if (!faCategories.TryGetValue(slug, out var faCategory))
                throw new InvalidOperationException($"Blog category '{slug}' exists in EN seed but not in FA seed.");

            items.Add(new BlogPostCategorySeedItem(
                Code: slug,
                FaTitle: faCategory.Label.Trim(),
                EnTitle: enCategory.Label.Trim(),
                FaSlug: slug,
                EnSlug: slug));
        }

        return items;
    }
}

internal sealed record BlogPostCategorySeedItem(
    string Code,
    string FaTitle,
    string EnTitle,
    string FaSlug,
    string EnSlug);

internal sealed class BlogSeedRoot
{
    public List<BlogCategorySeedItem> Categories { get; set; } = [];
}

internal sealed class BlogCategorySeedItem
{
    public string Id { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Label { get; set; } = default!;
}
