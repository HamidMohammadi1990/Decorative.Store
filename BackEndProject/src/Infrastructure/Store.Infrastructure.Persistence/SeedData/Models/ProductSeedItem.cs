namespace Store.Infrastructure.Persistence.SeedData.Models;

public sealed record ProductSeedItem(
    string ProductCode,
    string FaTitle,
    string EnTitle,
    string FaSlug,
    string EnSlug,
    string FaDescription,
    string EnDescription,
    string SubCategorySlug,
    string ImageBaseName,
    string FaImageTitle,
    string EnImageTitle);
