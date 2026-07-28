using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductProperties.Commands;

public record CreateProductPropertyResponse
{
    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int Id { get; init; }
}
