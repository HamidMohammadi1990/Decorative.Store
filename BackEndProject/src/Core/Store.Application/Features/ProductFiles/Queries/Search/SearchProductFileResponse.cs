using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductFiles.Queries;

public record SearchProductFileResponse
{
    [JsonConverter(typeof(ProductFileEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    
    public string FileName { get; init; } = default!;    
    public bool IsMain { get; init; }
}