namespace Store.Domain.Dtos.Products;

public record ProductSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string SubCategoryTitle { get; set; } = null!;
    public List<CheckoutProductImageDto> Images { get; set; } = [];
    public CheckoutProductImageDto? LayoutImage { get; set; }
}

public record CheckoutProductImageDto
{
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
}