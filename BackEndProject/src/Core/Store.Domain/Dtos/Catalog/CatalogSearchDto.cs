namespace Store.Domain.Dtos.Catalog;

public record CatalogSearchDto
{
    public List<CatalogSearchCategoryDto> Categories { get; init; } = [];
    public List<CatalogSearchSubCategoryDto> SubCategories { get; init; } = [];
    public List<CatalogListingProductDto> Products { get; init; } = [];
}

public record CatalogSearchCategoryDto
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public record CatalogSearchSubCategoryDto
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string CategoryTitle { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty;
}
