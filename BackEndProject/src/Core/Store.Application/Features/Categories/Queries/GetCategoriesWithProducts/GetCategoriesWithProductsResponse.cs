using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Categories.Queries;

public record GetCategoriesWithProductsResponse
{
    [JsonConverter(typeof(CategoryEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public List<SubCategoryWithProductsResponse> SubCategories { get; init; } = [];
}

public record SubCategoryWithProductsResponse
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public List<ProductForSubCategoryResponse> Products { get; init; } = [];
}

public record ProductForSubCategoryResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
}