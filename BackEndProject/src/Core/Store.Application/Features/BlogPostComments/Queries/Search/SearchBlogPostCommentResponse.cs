using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.BlogPostComments.Queries;

public record SearchBlogPostCommentResponse
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
    public string? CreatedByUserAvatarUrl { get; init; }

    [JsonConverter(typeof(BlogPostEncryptor))]
    public int BlogPostId { get; init; }

    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
}