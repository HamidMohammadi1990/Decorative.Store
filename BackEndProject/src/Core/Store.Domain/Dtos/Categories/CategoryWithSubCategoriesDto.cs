using Store.Domain.Dtos.SubCategories;

namespace Store.Domain.Dtos.Categories;

public record CategoryWithSubCategoriesDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public List<SubCategoryWithProductsDto> SubCategories { get; init; } = [];
}