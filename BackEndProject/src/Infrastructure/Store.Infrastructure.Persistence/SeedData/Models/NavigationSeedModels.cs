using System.Text.Json.Serialization;

namespace Store.Infrastructure.Persistence.SeedData.Models;

internal sealed class NavigationSeedRoot
{
    [JsonPropertyName("primaryNav")]
    public List<NavigationSeedItem> PrimaryNav { get; set; } = [];
}

internal sealed class NavigationSeedItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    [JsonPropertyName("href")]
    public string Href { get; set; } = default!;

    [JsonPropertyName("columns")]
    public List<NavigationSeedColumn>? Columns { get; set; }

    [JsonPropertyName("children")]
    public List<NavigationSeedLink>? Children { get; set; }
}

internal sealed class NavigationSeedColumn
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("links")]
    public List<NavigationSeedLink> Links { get; set; } = [];
}

internal sealed class NavigationSeedLink
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    [JsonPropertyName("href")]
    public string Href { get; set; } = default!;
}
