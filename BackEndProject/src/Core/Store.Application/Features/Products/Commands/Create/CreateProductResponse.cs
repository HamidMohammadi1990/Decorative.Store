using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Products.Commands;

public record CreateProductResponse
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
}