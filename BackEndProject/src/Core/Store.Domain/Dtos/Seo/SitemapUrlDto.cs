namespace Store.Domain.Dtos.Seo;

public record SitemapUrlDto
{
    public string Path { get; init; } = default!;
    public DateTime? LastModifiedUtc { get; init; }
    public string ChangeFrequency { get; init; } = "weekly";
    public decimal Priority { get; init; } = 0.5m;
}
