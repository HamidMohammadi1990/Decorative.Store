namespace Store.Domain.Dtos.PostTypes;

public record GetAllPostTypeResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public int Priority { get; init; }
}