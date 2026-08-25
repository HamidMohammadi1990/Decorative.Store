using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Products.Queries;

public record GetProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int SubCategoryId { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public bool IsActive { get; init; }
    public string Description { get; init; } = default!;
    public DateTime CreationDate { get; init; }
    public string ProductCode { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal? CompareAtPrice { get; init; }
}
