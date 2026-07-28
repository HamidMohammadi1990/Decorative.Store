using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.PropertyItemPrices;

public record GetAllPropertyItemPriceRequestDto : IContentPolicyQueryDto<PropertyItemPrice>
{
    [QueryFilter(MemberPath = "propertyItemPrice.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "property.Id")]
    public int? PropertyId { get; init; }

    [QueryFilter(MemberPath = "propertyCategory.Id")]
    public int? PropertyCategoryId { get; init; }

    [QueryFilter(MemberPath = "propertyItemPrice.PropertyItemId")]
    public int? PropertyItemId { get; init; }

    [QueryFilter(MemberPath = "propertyItemPrice.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<PropertyItemPrice, bool>>? ContentFilter { get; set; }
}
