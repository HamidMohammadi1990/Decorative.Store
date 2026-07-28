using System.Text.Json.Serialization;
using Edition.Application.Features.CompanyStories;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStories.Queries;

public record SearchCompanyStoryResponse
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string CompanyName { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }

    public string CreatedByUserFirstName { get; init; } = default!;
    public string CreatedByUserLastName { get; init; } = default!;
    public string? Caption { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ExpiresAtUtc { get; init; }
    public int LikeCount { get; init; }
    public int CommentCount { get; init; }
    public List<CompanyStoryItemResponse> Items { get; init; } = [];
}
