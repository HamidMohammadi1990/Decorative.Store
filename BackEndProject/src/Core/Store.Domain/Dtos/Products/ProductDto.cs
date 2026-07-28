namespace Store.Domain.Dtos.Products;

public record ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
}