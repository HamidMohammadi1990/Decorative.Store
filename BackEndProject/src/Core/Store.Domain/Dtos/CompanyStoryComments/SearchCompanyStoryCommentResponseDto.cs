namespace Store.Domain.Dtos.CompanyStoryComments;

public record SearchCompanyStoryCommentResponseDto
{
    public int Id { get; init; }
    public string Content { get; init; } = default!;
    public int? ParentId { get; init; }
    public int CompanyStoryId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public string? CreatedByUserFirstName { get; init; }
    public string? CreatedByUserLastName { get; init; }
    public int CreatedByUserId { get; init; }
}
