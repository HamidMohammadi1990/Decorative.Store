using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public record GetAllCompanyStoryLikeResponse
{
    [JsonConverter(typeof(CompanyStoryLikeEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    public string? CompanyStoryCaption { get; init; }
    public string? UserName { get; init; }
    public string ClientIP { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
}
