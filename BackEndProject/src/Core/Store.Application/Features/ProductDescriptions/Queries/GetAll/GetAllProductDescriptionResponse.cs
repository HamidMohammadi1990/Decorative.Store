using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public record GetAllProductDescriptionResponse
{
    [JsonConverter(typeof(ProductDescriptionEncryptor))]
    public int Id { get; init; }

    public string Description { get; init; } = default!;

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;
}