using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductComments.Queries;

public record SearchProductCommentResponse
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int CommentTopicId { get; init; }

    public string CommentTopicTitle { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = string.Empty;

    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string UserName { get; init; } = default!;

    public int CommentRate { get; init; }
    public string? Description { get; init; }
    public int QualityRating { get; init; }
    public int AffordableRating { get; init; }
    public bool IsBuyer { get; init; }
    public int HelpfulCount { get; init; }
    public int NotHelpfulCount { get; init; }
}