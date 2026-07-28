using Newtonsoft.Json;

namespace Store.Infrastructure.Persistence.Models;

internal record ChartOfAccountSeedDataDto
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("parentId")]
    public int ParentId { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("parentPath")]
    public string ParentPath { get; set; } = default!;

    [JsonProperty("accountType")]
    public int AccountType { get; set; }

    [JsonProperty("detailType")]
    public int DetailType { get; set; }
}