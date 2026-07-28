using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Commands;

public record UpdateProductCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CommentTopicEncryptor))]
    public int CommentTopicId { get; init; }

    public int CommentRate { get; init; }
    public string Description { get; init; } = default!;
    public int QualityRating { get; init; }
    public int AffordableRating { get; init; }    
}