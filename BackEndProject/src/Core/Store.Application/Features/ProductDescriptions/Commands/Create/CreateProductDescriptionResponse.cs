using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public record CreateProductDescriptionResponse
{
    [JsonConverter(typeof(ProductDescriptionEncryptor))]
    public int Id { get; init; }
}