using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryLikes.Commands;

public record CreateCompanyStoryLikeResponse
{
    [JsonConverter(typeof(CompanyStoryLikeEncryptor))]
    public int Id { get; init; }
}
