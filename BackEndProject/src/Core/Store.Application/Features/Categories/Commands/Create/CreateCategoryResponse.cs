using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Categories.Commands;

public record CreateCategoryResponse
{
    [JsonConverter(typeof(CategoryEncryptor))]
    public int Id { get; init; }
}