using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductComments;

public record SearchProductCommentRequestDto : IContentPolicyQueryDto<ProductComment>
{
    [QueryFilter(MemberPath = "productComment.ProductId")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "productComment.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "productComment.CommentTopicId")]
    public int? CommentTopicId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductComment, bool>>? ContentFilter { get; set; }
}
