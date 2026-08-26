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

    public string? Title { get; init; }

    public string? Slug { get; init; }

    [QueryFilter(MemberPath = "blogPost.IsFeatured")]
    public bool? IsFeatured { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<BlogPost, bool>>? ContentFilter { get; set; }
}
