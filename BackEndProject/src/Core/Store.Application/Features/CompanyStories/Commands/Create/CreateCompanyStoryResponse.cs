using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStories.Commands;

public record CreateCompanyStoryResponse
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }
}
