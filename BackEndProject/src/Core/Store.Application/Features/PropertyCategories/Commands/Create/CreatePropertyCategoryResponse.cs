using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.PropertyCategories.Commands;

public record CreatePropertyCategoryResponse
{
    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int Id { get; init; }
}