using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Cities;

public record SearchCityRequestDto : IContentPolicyQueryDto<City>
{
    [QueryFilter]
    public int? ProvinceId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter]
    public string? Slug { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<City, bool>>? ContentFilter { get; set; }
}
