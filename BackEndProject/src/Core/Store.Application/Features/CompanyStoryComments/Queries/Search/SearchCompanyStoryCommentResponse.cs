using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record SearchCompanyStoryCommentResponse
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }

    public string Content { get; init; } = default!;
    public string CreatedByUserFirstName { get; init; } = default!;
    public string CreatedByUserLastName { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
}
