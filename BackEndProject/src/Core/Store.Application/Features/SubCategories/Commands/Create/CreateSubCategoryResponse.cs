using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.SubCategories.Commands;

public record CreateSubCategoryResponse
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }
}