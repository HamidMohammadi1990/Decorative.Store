using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record GetAllCompanyStoryCommentResponse
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    public string? CompanyStoryCaption { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; init; }

    public string Content { get; init; } = default!;
    public string CreatedByUserFirstName { get; init; } = default!;
    public string CreatedByUserLastName { get; init; } = default!;
    public string? ApprovedByUserFirstName { get; init; }
    public string? ApprovedByUserLastName { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public bool IsApproved { get; init; }
}
