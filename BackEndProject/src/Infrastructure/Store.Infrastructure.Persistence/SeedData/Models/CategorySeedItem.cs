namespace Store.Infrastructure.Persistence.SeedData.Models;

public sealed record CategorySeedItem(
    string Code,
    string FaTitle,
    string EnTitle,
    string FaSlug,
    string EnSlug,
    IReadOnlyList<SubCategorySeedItem> SubCategories);

public sealed record SubCategorySeedItem(
    string Code,
    string FaTitle,
    string EnTitle,
    string FaSlug,
    string EnSlug);
