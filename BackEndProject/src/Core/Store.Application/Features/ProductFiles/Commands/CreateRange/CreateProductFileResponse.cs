using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductFiles.Commands;

public record CreateProductFileResponse
{
    [JsonConverter(typeof(ProductFileEncryptor))]
    public int Id { get; init; }
    public string? Title { get; init; }
    public string ImageUrl { get; init; } = default!;
}