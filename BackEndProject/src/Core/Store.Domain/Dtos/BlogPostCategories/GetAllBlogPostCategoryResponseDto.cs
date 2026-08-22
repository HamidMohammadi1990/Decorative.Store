namespace Store.Domain.Dtos.BlogPostCategories;

public record GetAllBlogPostCategoryResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
    public IReadOnlyList<Store.Domain.Dtos.Localization.TranslationItemDto> Translations { get; init; } = [];
}
