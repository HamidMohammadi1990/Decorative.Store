using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Categories;

public record GetAllCategoryRequestDto : IContentPolicyQueryDto<Category>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Code { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Category, bool>>? ContentFilter { get; set; }
}
