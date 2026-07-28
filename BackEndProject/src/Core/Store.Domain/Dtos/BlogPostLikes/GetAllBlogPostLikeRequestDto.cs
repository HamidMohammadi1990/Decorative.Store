using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.BlogPostLikes;

public record GetAllBlogPostLikeRequestDto : IContentPolicyQueryDto<BlogPostLike>
{
    [QueryFilter(MemberPath = "blogPostLike.BlogPostId")]
    public int? BlogPostId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPostLike, bool>>? ContentFilter { get; set; }
}
