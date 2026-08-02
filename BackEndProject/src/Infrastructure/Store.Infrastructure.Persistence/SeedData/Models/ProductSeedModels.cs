using System.Text.Json.Serialization;

namespace Store.Infrastructure.Persistence.SeedData.Models;

internal sealed class ProductSeedJsonItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = default!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("image")]
    public ProductSeedImage Image { get; set; } = default!;

    [JsonPropertyName("categorySlugs")]
    public List<string> CategorySlugs { get; set; } = [];

    [JsonPropertyName("subcategorySlug")]
    public string? SubCategorySlug { get; set; }
}

internal sealed class ProductSeedImage
{
    [JsonPropertyName("src")]
    public string Src { get; set; } = default!;

    [JsonPropertyName("alt")]
    public string Alt { get; set; } = default!;
}
