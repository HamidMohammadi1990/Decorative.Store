using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record GetCompanyStoryCommentResponse
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; init; }

    public string Content { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public bool IsApproved { get; init; }
}
