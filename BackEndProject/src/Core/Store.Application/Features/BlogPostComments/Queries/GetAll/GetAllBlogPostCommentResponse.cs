using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostComments.Queries;

public record GetAllBlogPostCommentResponse
{
    [JsonConverter(typeof(BlogPostCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(BlogPostCommentNullableEncryptor))]
    public int? ParentId { get; init; }
    public string Content { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int CreatedByUserId { get; init; }
    public string CreatedByUserFirstName { get; init; } = default!;
    public string CreatedByUserLastName { get; init; } = default!;

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; init; }
    public string? ApprovedByUserFirstName { get; init; }
    public string? ApprovedByUserLastName { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public string BlogPostTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public bool IsApproved { get; init; }
}