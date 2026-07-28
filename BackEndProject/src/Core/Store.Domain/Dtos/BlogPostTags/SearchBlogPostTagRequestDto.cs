using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.BlogPostTags;

public record SearchBlogPostTagRequestDto : IContentPolicyQueryDto<BlogPostTag>
{
    [QueryFilter(MemberPath = "blogPost.Id")]
    public int? BlogPostId { get; init; }

    [QueryFilter(MemberPath = "tag.Id")]
    public int? TagId { get; init; }

    [QueryFilter(MemberPath = "tag.Title", Operator = FilterOperator.Contains)]
    public string? TagTitle { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPostTag, bool>>? ContentFilter { get; set; }
}
