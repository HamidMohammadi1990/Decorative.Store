using System.Text.Json;
using Store.Infrastructure.Persistence.SeedData.Models;

namespace Store.Infrastructure.Persistence.SeedData;

internal static class CategorySeedData
{
    private const string FaFileName = "navigation.fa.json";
    private const string EnFileName = "navigation.en.json";
    private const string BlogNavId = "blog";
    private const int MaxCodeLength = 12;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static IReadOnlyList<CategorySeedItem> Items { get; } = Build();

    private static IReadOnlyList<CategorySeedItem> Build()
    {
        var seedDataPath = Path.Combine(AppContext.BaseDirectory, "SeedData");
        var faJson = File.ReadAllText(Path.Combine(seedDataPath, FaFileName));
        var enJson = File.ReadAllText(Path.Combine(seedDataPath, EnFileName));

        var faNavigation = JsonSerializer.Deserialize<NavigationSeedRoot>(faJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {FaFileName}.");
        var enNavigation = JsonSerializer.Deserialize<NavigationSeedRoot>(enJson, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {EnFileName}.");

        var faItems = faNavigation.PrimaryNav
            .Where(x => !string.Equals(x.Id, BlogNavId, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

        var enItems = enNavigation.PrimaryNav
            .Where(x => !string.Equals(x.Id, BlogNavId, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

        var categories = new List<CategorySeedItem>();

        foreach (var (id, faItem) in faItems)
        {
            if (!enItems.TryGetValue(id, out var enItem))
                throw new InvalidOperationException($"Navigation item '{id}' exists in FA seed but not in EN seed.");

            categories.Add(new CategorySeedItem(
                Code: ToCode(id),
                FaTitle: faItem.Label.Trim(),
                EnTitle: enItem.Label.Trim(),
                FaSlug: ToSlug(faItem.Href),
                EnSlug: ToSlug(enItem.Href),
                SubCategories: BuildSubCategories(faItem, enItem)));
        }

        return categories;
    }

    private static List<SubCategorySeedItem> BuildSubCategories(NavigationSeedItem faItem, NavigationSeedItem enItem)
    {
        var faLinks = CollectLinks(faItem);
        var enLinks = CollectLinks(enItem).ToDictionary(x => x.Href, x => x.Label, StringComparer.OrdinalIgnoreCase);
        var usedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var subCategories = new List<SubCategorySeedItem>();

        foreach (var faLink in faLinks.DistinctBy(x => x.Href, StringComparer.OrdinalIgnoreCase))
        {
            if (!enLinks.TryGetValue(faLink.Href, out var enLabel))
                enLabel = faLink.Label;

            var slug = ToSlug(faLink.Href);
            var code = CreateUniqueCode(slug, usedCodes);

            subCategories.Add(new SubCategorySeedItem(
                Code: code,
                FaTitle: faLink.Label.Trim(),
                EnTitle: enLabel.Trim(),
                FaSlug: slug,
                EnSlug: slug));
        }

        return subCategories;
    }

    private static List<NavigationSeedLink> CollectLinks(NavigationSeedItem item)
    {
        var links = new List<NavigationSeedLink>();

        if (item.Columns is not null)
        {
            foreach (var column in item.Columns)
                links.AddRange(column.Links);
        }

        if (item.Children is not null)
            links.AddRange(item.Children);

        return links;
    }

    private static string ToSlug(string href)
        => href.Trim().Trim('/');

    private static string ToCode(string value)
    {
        var code = value.Trim().Replace('/', '-');
        return code.Length <= MaxCodeLength ? code : code[..MaxCodeLength];
    }

    private static string CreateUniqueCode(string slug, ISet<string> usedCodes)
    {
        var baseCode = ToCode(slug);
        if (usedCodes.Add(baseCode))
            return baseCode;

        for (var index = 2; index < 100; index++)
        {
            var suffix = index.ToString();
            var candidate = baseCode.Length + suffix.Length <= MaxCodeLength
                ? $"{baseCode}{suffix}"
                : $"{baseCode[..(MaxCodeLength - suffix.Length)]}{suffix}";

            if (usedCodes.Add(candidate))
                return candidate;
        }

        throw new InvalidOperationException($"Unable to generate unique code for slug '{slug}'.");
    }
}
