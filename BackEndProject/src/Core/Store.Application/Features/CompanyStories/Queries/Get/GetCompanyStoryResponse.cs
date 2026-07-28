using System.Text.Json.Serialization;
using Edition.Application.Features.CompanyStories;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStories.Queries;

public record GetCompanyStoryResponse
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }

    public string? Caption { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? ExpiresAtUtc { get; init; }
    public bool IsActive { get; init; }
    public List<CompanyStoryItemResponse> Items { get; init; } = [];
}
