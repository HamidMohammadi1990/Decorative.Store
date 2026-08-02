using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Commands;

public record CreateProductCommentRequest : IRequest<OperationResult<CreateProductCommentResponse>>
{
    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int CommentTopicId { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public int CommentRate { get; init; }
    public string Description { get; init; } = default!;
    public int QualityRating { get; init; }
    public int AffordableRating { get; init; }
}