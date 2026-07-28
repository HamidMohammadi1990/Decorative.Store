using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Products.Queries;

public record GetProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
    public string Description { get; init; } = default!;
    public DateTime CreationDate { get; init; }
    public string ProductCode { get; init; } = default!;
}