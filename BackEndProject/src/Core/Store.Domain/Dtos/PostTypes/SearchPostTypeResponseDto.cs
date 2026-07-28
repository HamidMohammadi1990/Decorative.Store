namespace Store.Domain.Dtos.PostTypes;

public record SearchPostTypeResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }    
    public int Priority { get; init; }
}