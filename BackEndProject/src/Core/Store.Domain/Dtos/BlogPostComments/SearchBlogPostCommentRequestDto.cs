using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.BlogPostComments;

public record SearchBlogPostCommentRequestDto : IContentPolicyQueryDto<BlogPostComment>
{
    [QueryFilter(MemberPath = "blogPostComment.BlogPostId")]
    public int BlogPostId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPostComment, bool>>? ContentFilter { get; set; }
}
