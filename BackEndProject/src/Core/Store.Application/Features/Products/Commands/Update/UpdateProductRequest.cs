using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Products.Commands;

public record UpdateProductRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }

    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int SubCategoryId { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public bool Status { get; init; }
    public string Description { get; init; } = default!;
    public string ProductCode { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal? CompareAtPrice { get; init; }
}
