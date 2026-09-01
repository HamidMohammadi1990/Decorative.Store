namespace Store.Domain.Dtos.BlogPostFiles;

public record BlogPostImageDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string FileName { get; init; } = default!;
    public bool IsMain { get; init; }
}
