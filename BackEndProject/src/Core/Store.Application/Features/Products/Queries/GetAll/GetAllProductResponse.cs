using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Features.Localization;

namespace Edition.Application.Features.Products.Queries;

public record GetAllProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
    public string ProductCode { get; init; } = default!;
    public bool IsActive { get; init; }
    public DateTime CreationDate { get; init; }

    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int SubCategoryId { get; init; }

    public IReadOnlyList<ProductTranslationItemResponse> Translations { get; init; } = [];
}
