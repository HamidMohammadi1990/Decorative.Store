using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.ContentPolicies;

namespace Store.Domain.Dtos.ProductPropertyRules;

public record GetAllProductPropertyRuleRequestDto : IContentPolicyQueryDto<ProductPropertyRule>
{
    [QueryFilter]
    public int? ProductPropertyId { get; init; }

    [QueryFilter]
    public PropertyType? PropertyType { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductPropertyRule, bool>>? ContentFilter { get; set; }
}