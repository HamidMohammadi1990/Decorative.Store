using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.CommentTopics;

public record GetAllCommentTopicRequestDto : IContentPolicyQueryDto<CommentTopic>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; } = true;

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<CommentTopic, bool>>? ContentFilter { get; set; }
}
