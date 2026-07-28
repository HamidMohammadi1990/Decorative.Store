using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.BlogPosts;

public record SearchBlogPostRequestDto : IContentPolicyQueryDto<BlogPost>
{
    [QueryFilter(MemberPath = "blogPost.BlogPostCategoryId")]
    public int? CategoryId { get; init; }

    [QueryFilter(MemberPath = "blogPost.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "blogPost.Slug", Operator = FilterOperator.Contains)]
    public string? Slug { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPost, bool>>? ContentFilter { get; set; }
}
