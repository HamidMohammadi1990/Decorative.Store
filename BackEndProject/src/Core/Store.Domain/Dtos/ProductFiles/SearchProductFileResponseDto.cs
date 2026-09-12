using Store.Domain.Enums;

namespace Store.Domain.Dtos.ProductFiles;

public record SearchProductFileResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int ProductId { get; init; }    
    public string FileName { get; init; } = default!;    
    public bool IsMain { get; init; }
    public ProductFileKind Kind { get; init; }
}