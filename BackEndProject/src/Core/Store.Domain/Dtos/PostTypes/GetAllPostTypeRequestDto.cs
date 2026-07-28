using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.PostTypes;

public record GetAllPostTypeRequestDto : IContentPolicyQueryDto<PostType>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; } = true;

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<PostType, bool>>? ContentFilter { get; set; }
}
