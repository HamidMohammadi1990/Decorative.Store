using Store.Domain.Dtos.Products;

namespace Store.Domain.Dtos.SubCategories;

public record SubCategoryWithProductsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = [];
}