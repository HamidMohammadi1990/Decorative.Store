using Store.Domain.Dtos.Localization;

namespace Store.Domain.Dtos.Products;

public record GetAllProductResponseDto
{
    public int Id { get; init; }
    public string ProductCode { get; init; } = default!;
    public bool IsActive { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public int SubCategoryId { get; init; }
    public IReadOnlyList<ProductTranslationItemDto> Translations { get; init; } = [];
}
