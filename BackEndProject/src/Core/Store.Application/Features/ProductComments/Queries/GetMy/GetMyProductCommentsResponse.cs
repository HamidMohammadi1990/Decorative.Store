using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductComments.Queries;

public record GetMyProductCommentsResponse
{
    public List<GetMyProductCommentItemResponse> Items { get; init; } = [];
}

public record GetMyProductCommentItemResponse
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    public string ProductTitle { get; init; } = default!;

    public string ProductSlug { get; init; } = default!;

    public int CommentRate { get; init; }

    public string Description { get; init; } = default!;

    public bool IsActive { get; init; }
}
