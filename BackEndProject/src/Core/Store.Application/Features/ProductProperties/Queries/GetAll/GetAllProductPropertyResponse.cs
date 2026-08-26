using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductProperties.Queries;

public record GetAllProductPropertyResponse
{
    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }

    [JsonConverter(typeof(PropertyItemNullableEncryptor))]
    public int? PropertyItemId { get; init; }

    public bool IsActive { get; init; }
}
