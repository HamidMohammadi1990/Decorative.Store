using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Properties;

public record GetAllPropertyRequestDto : IContentPolicyQueryDto<Property>
{
    [QueryFilter(MemberPath = "property.ParentId")]
    public int? ParentId { get; init; }

    [QueryFilter(MemberPath = "property.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "property.PropertyCategoryId")]
    public int? PropertyCategoryId { get; init; }

    [QueryFilter(MemberPath = "property.PropertyType")]
    public PropertyType? PropertyType { get; init; }

    [QueryFilter(MemberPath = "property.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Property, bool>>? ContentFilter { get; set; }
}
