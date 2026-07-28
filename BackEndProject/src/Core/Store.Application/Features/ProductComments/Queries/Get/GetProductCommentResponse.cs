using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductComments.Queries;

public record GetProductCommentResponse
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public int CommentRate { get; init; }
    public int QualityRating { get; init; }
    public int CommentTopicId { get; init; }
    public string? Description { get; init; }
    public int AffordableRating { get; init; }
}