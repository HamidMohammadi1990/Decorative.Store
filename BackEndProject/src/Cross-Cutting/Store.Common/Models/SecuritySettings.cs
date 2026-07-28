namespace Store.Common.Models;

public record SecuritySettings
{
    public string GeneralKey { get; init; } = string.Empty;
    public Dictionary<string, string> EntityKeys { get; init; } = new(StringComparer.Ordinal);
}
