using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public record GetCompanyStoryLikeResponse
{
    [JsonConverter(typeof(CompanyStoryLikeEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public string ClientIP { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
}
