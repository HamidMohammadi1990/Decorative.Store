namespace Store.Domain.Dtos.ProductQuestions;

public record GetAllProductQuestionResponseDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string UserName { get; init; } = default!;
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public string Question { get; init; } = default!;
    public string? Answer { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
    public string? AnsweredByFirstName { get; init; }
    public string? AnsweredByLastName { get; init; }
    public string? AnsweredByUserName { get; init; }
}
