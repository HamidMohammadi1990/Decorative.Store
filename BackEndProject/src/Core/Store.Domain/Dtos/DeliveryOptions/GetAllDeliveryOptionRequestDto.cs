using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.DeliveryOptions;

public record GetAllDeliveryOptionRequestDto : IContentPolicyQueryDto<DeliveryOption>
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter]
    public int? DeliveryDays { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<DeliveryOption, bool>>? ContentFilter { get; set; }
}
