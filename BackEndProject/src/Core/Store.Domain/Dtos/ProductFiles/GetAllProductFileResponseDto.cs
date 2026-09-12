using Store.Domain.Enums;

namespace Store.Domain.Dtos.ProductFiles;

public record GetAllProductFileResponseDto
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public int ProductId { get; init; }
    public string ProductTitle { get; init; } = default!;
    public string FileName { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsMain { get; init; }
    public ProductFileKind Kind { get; init; }
}