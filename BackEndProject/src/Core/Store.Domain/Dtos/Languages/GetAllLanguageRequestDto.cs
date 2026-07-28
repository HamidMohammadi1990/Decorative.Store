using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Languages;

public record GetAllLanguageRequestDto
{
    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Code { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Name { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    [QueryFilter]
    public bool? IsDefault { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
