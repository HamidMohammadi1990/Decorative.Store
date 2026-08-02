using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.PropertyItems;

public record GetAllPropertyItemRequestDto : IContentPolicyQueryDto<PropertyItem>
{
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "propertyItem.PropertyId")]
    public int? PropertyId { get; init; }

    [QueryFilter(MemberPath = "propertyItem.IsActive")]
    public bool? IsActive { get; init; }

    public string? PropertyTitle { get; init; }

    [QueryFilter(MemberPath = "property.PropertyCategoryId")]
    public int? PropertyCategoryId { get; init; }

    [QueryFilter(MemberPath = "property.PropertyType")]
    public PropertyType? PropertyType { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<PropertyItem, bool>>? ContentFilter { get; set; }
}
