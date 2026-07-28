using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductComments.Queries;

public record SearchProductCommentResponse
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string CompanyName { get; init; } = default!;

    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int CommentTopicId { get; init; }

    public string CommentTopicTitle { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string ProductTitle { get; init; } = default!;

    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string UserName { get; init; } = default!;

    public int CommentRate { get; init; }
    public string? Description { get; init; }
    public int QualityRating { get; init; }
    public int AffordableRating { get; init; }
}