using Newtonsoft.Json;

namespace Store.Infrastructure.Persistence.Models;

internal record ProvinceSeedDataDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("slug")]
    public string Slug { get; set; } = default!;

    [JsonProperty("tel_prefix")]
    public string TelPrefix { get; set; } = default!;
}