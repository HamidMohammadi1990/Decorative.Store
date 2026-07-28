namespace Store.Infrastructure.Persistence.Models;

internal record CategoriesSeedDataDto(string Title, string Code, string Slug, List<SubCategorySeedDataDto> SubCategories);
internal record SubCategorySeedDataDto(string Title, string Code, string Slug, List<ProductSeedDataDto> Products);
internal record ProductSeedDataDto(string Title, string Slug, string Code);
internal record AllProductsSeedDataDto(string CategoryTitle, List<ProductSeedDataDto> Products);

internal record ProductPropertySeedDto(string CategoryTitle, List<PropertySeedDto> Properties);

public record PropertySeedDto
    (string Title, string Type, bool IsRequired, int Min,
     int Max, List<PropertyItemSeedDto> Items, List<PropertyDependencySeedDto> Dependencies,
     List<PropertySeedDto> Properties, string Description);

public record PropertyItemSeedDto(string Id, string Title, string type, string typeTitle, int Min, int Max);

public record PropertyDependencySeedDto
{
    public string Title { get; set; }
    public string Type { get; set; }
    public bool IsRequired { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public List<DependencyItemSeedDto> Items { get; set; }
    public List<PropertyDependencySeedDto> Dependencies { get; set; }
    public List<PropertySeedDto> Properties { get; set; }
}

public record DependencyItemSeedDto
{
    public string Id { get; set; }
    public string Title { get; set; }
}