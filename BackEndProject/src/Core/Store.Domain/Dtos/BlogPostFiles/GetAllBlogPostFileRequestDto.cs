using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.BlogPostFiles;

public record GetAllBlogPostFileRequestDto : IContentPolicyQueryDto<BlogPostFile>
{
    public string? Title { get; init; }
    public int? BlogPostId { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsMain { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
    public Expression<Func<BlogPostFile, bool>>? ContentFilter { get; set; }
}
