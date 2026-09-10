namespace Store.Domain.Dtos.BlogPosts;

public record SearchBlogPostDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string CategoryTitle { get; set; } = default!;
    public int CategoryId { get; init; }
    public string MetaDescription { get; init; } = default!;
    public string SeoKeywords { get; init; } = default!;
    public string Content { get; init; } = default!;
    public string UserFirstName { get; set; } = default!;
    public string UserLastName { get; set; } = default!;
    public string? UserProfileImageFileName { get; init; }
    public int UserId { get; init; }
    public int ReadingTimeInMinutes { get; set; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
    public bool IsActive { get; init; }
    public bool IsPublished { get; init; }
    public bool IsFeatured { get; init; }
}