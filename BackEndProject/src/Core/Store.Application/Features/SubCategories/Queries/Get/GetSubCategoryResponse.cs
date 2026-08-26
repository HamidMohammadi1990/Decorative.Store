using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.SubCategories.Queries;

public record GetSubCategoryResponse
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;

    [JsonConverter(typeof(CategoryEncryptor))]
    public int CategoryId { get; init; }

    public bool IsActive { get; init; }
}