namespace Store.Domain.Dtos.CompanyComments;

public record SearchCompanyCommentResponseDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int Rate { get; init; }    
}