using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductFiles.Queries;

public record GetProductFileResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string? Title { get; init; }
    public string FileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}