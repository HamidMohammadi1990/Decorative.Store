namespace Store.Common.Models;

public record StorefrontSettings
{
    public string BaseUrl { get; init; } = "http://localhost:5173";
}
