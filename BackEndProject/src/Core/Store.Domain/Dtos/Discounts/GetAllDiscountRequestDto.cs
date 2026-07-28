using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Discounts;

public record GetAllDiscountRequestDto : IContentPolicyQueryDto<Discount>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Code { get; init; }

    [QueryFilter]
    public int? ProductId { get; init; }

    [QueryFilter]
    public int? UserId { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Discount, bool>>? ContentFilter { get; set; }
}
