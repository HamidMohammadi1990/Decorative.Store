using Newtonsoft.Json;

namespace Store.Infrastructure.Persistence.Models;

internal record CitySeedDataDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("slug")]
    public string Slug { get; set; } = default!;

    [JsonProperty("province_id")]
    public int ProvinceId { get; set; }
}