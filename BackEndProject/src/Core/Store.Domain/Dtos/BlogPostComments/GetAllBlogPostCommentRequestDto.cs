using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.BlogPostComments;

public record GetAllBlogPostCommentRequestDto : IContentPolicyQueryDto<BlogPostComment>
{
    [QueryFilter(MemberPath = "blogPost.Id")]
    public int? BlogPostId { get; init; }

    [QueryFilter(MemberPath = "blogPostComment.CreatedByUserId")]
    public int? CreatedByUserId { get; init; }

    [QueryFilter(MemberPath = "blogPostComment.ApprovedByUserId")]
    public int? ApprovedByUserId { get; init; }

    [QueryFilter(MemberPath = "blogPostComment.IsApproved")]
    public bool? IsApproved { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPostComment, bool>>? ContentFilter { get; set; }
}
