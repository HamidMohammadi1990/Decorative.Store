using System.Text.Json;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class BlogPostSeedData
{
    private const string FaFileName = "blog.fa.json";
    private const string EnFileName = "blog.en.json";
    private const int MaxContentLength = 2500;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static IReadOnlyList<BlogPostSeedItem> Items { get; } = Build();

    private static IReadOnlyList<BlogPostSeedItem> Build()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var faJson = File.ReadAllText(Path.Combine(seedDataPath, FaFileName));
        var enJson = File.ReadAllText(Path.Combine(seedDataPath, EnFileName));

        var faBlog = JsonSerializer.Deserialize<BlogPostFileRoot>(faJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {FaFileName}.");
        var enBlog = JsonSerializer.Deserialize<BlogPostFileRoot>(enJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {EnFileName}.");

        var faPosts = faBlog.Posts.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        var items = new List<BlogPostSeedItem>();

        foreach (var enPost in enBlog.Posts)
        {
            if (!faPosts.TryGetValue(enPost.Id, out var faPost))
                throw new InvalidOperationException($"Blog post '{enPost.Id}' exists in EN seed but not in FA seed.");

            if (!DateTime.TryParse(enPost.PublishedAt, out var publishedOnUtc))
                publishedOnUtc = DateTime.UtcNow;

            items.Add(new BlogPostSeedItem(
                Code: enPost.Id,
                CategorySlug: enPost.CategorySlug,
                FaTitle: faPost.Title.Trim(),
                EnTitle: enPost.Title.Trim(),
                FaSlug: faPost.Slug.Trim(),
                EnSlug: enPost.Slug.Trim(),
                FaMetaDescription: faPost.Excerpt.Trim(),
                EnMetaDescription: enPost.Excerpt.Trim(),
                FaContent: ToContent(faPost.Content),
                EnContent: ToContent(enPost.Content),
                FaSeoKeywords: ToSeoKeywords(faPost.Tags),
                EnSeoKeywords: ToSeoKeywords(enPost.Tags),
                ReadingTimeMinutes: enPost.ReadTimeMinutes,
                IsFeatured: enPost.Featured,
                PublishedOnUtc: DateTime.SpecifyKind(publishedOnUtc, DateTimeKind.Utc)));
        }

        return items;
    }

    private static string ToContent(IReadOnlyList<string> paragraphs)
    {
        var content = string.Join("\n\n", paragraphs.Select(x => x.Trim()).Where(x => x.Length > 0));
        return content.Length <= MaxContentLength ? content : content[..MaxContentLength];
    }

    private static string ToSeoKeywords(IReadOnlyList<string> tags)
        => string.Join(", ", tags.Select(x => x.Trim()).Where(x => x.Length > 0));
}

internal sealed record BlogPostSeedItem(
    string Code,
    string CategorySlug,
    string FaTitle,
    string EnTitle,
    string FaSlug,
    string EnSlug,
    string FaMetaDescription,
    string EnMetaDescription,
    string FaContent,
    string EnContent,
    string FaSeoKeywords,
    string EnSeoKeywords,
    int ReadingTimeMinutes,
    bool IsFeatured,
    DateTime PublishedOnUtc);

internal sealed class BlogSeedPostItem
{
    public string Id { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Excerpt { get; set; } = default!;
    public string CategorySlug { get; set; } = default!;
    public string PublishedAt { get; set; } = default!;
    public int ReadTimeMinutes { get; set; }
    public bool Featured { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<string> Content { get; set; } = [];
}

internal sealed class BlogPostFileRoot
{
    public List<BlogSeedPostItem> Posts { get; set; } = [];
}
