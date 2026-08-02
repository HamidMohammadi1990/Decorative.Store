using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Products.Commands;

public record CreateProductRequest : IRequest<OperationResult<CreateProductResponse>>
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string ProductCode { get; init; } = default!;

    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int SubCategoryId { get; init; }
}
